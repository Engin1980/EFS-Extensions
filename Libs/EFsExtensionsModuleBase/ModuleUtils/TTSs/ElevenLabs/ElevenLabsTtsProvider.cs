﻿using ESystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static ESystem.Functions.TryCatch;
using System.Windows.Markup;
using System.Security.Policy;
using System.Linq.Expressions;

namespace Eng.EFsExtensions.EFsExtensionsModuleBase.ModuleUtils.TTSs.ElevenLabs
{
  public class ElevenLabsTtsProvider : ITtsProvider
  {

    #region Fields

    private const string URL = "https://api.elevenlabs.io/v1";
    private const string URL_SPEECH_SUBROUTE = "text-to-speech";
    private const string URL_VOICES_SUBROUTE = "voices";
    private const string URL_MODELS_SUBROUTE = "models";
    private readonly HttpClient http;
    private readonly ElevenLabsTtsSettings settings;
    private readonly Dictionary<string, byte[]> previousSpeeches = new();

    #endregion Fields

    #region Constructors

    public ElevenLabsTtsProvider(ElevenLabsTtsSettings settings)
    {
      this.settings = settings;
      this.http = GetHttpClient(settings.ApiKey);
    }

    #endregion Constructors

    #region Methods

    public async Task<byte[]> ConvertAsync(string text)
    {
      if (previousSpeeches.ContainsKey(text) == false)
      {
        string url = $"{URL}/{URL_SPEECH_SUBROUTE}/{settings.VoiceId}";
        string body = BuildHttpGetModelJson(text, settings.Stability, settings.Similarity, settings.Style);
        var tmp = await DownloadSpeechAsync(this.http, url, body);
        previousSpeeches[text] = tmp;
      }
      byte[] ret = previousSpeeches[text];
      return ret;
    }

    public byte[] Convert(string text)
    {
      return ConvertAsync(text).GetAwaiter().GetResult();
    }

    public static async Task<List<ElevenLabsVoice>> GetVoicesAsync(string apiKey)
    {
      var httpClient = ElevenLabsTtsProvider.GetHttpClient(apiKey);
      string url = $"{URL}/{URL_VOICES_SUBROUTE}";
      VoicesResponse tmp = await DownloadVoicesAsync(httpClient, url);
      List<ElevenLabsVoice> ret = tmp.Voices.ToList();
      return ret;
    }

    public static async Task<List<string>> GetModelsAsync(string apiKey)
    {
      var httpClient = ElevenLabsTtsProvider.GetHttpClient(apiKey);
      string url = $"{URL}/{URL_MODELS_SUBROUTE}";
      List<string> tmp = await DownloadModelsAsync(httpClient, url);
      List<string> ret = tmp;
      return ret;
    }

    public static HttpClient GetHttpClient(string api)
    {
      var ret = new HttpClient();
      ret.DefaultRequestHeaders.Add("xi-api-key", api);
      return ret;
    }

    private static async Task<VoicesResponse> DownloadVoicesAsync(HttpClient httpClient, string url)
    {
      var response = await httpClient.GetAsync(url);

      if (response.StatusCode != System.Net.HttpStatusCode.OK)
        throw new TtsApplicationException("Failed to download voices.",
          new ApplicationException($"GET request returned {response.StatusCode}:{response.Content}."));

      var tmp = await Try(
        async () => await response.Content.ReadAsStringAsync(),
        ex => new TtsApplicationException("Failed to read POST response stream.", ex));

      tmp = NormalizeSnakeCaseToUpperCase(tmp);

      VoicesResponse ret = JsonConvert.DeserializeObject<VoicesResponse>(tmp)!;

      return ret;
    }

    private static string NormalizeSnakeCaseToUpperCase(string txt)
    {
      string p = @"""([^""]+?)"" *:";
      var matches = System.Text.RegularExpressions.Regex.Matches(txt, p);
      var matchesOrdered = matches.Where(q => q.Success).Where(q => q.Groups[1].Value.Contains("_")).OrderByDescending(q => q.Index);
      string convertSnakeToUpper(string src)
      {
        bool nextUpper = true;
        StringBuilder ret = new StringBuilder();
        for (int i = 0; i < src.Length; i++)
        {
          char c = src[i];
          if (c == '_')
            nextUpper = true;
          else
          {
            if (nextUpper)
            {
              ret.Append(char.ToUpper(c));
              nextUpper = false;
            }
            else
              ret.Append(c);
          }
        }
        return ret.ToString();
      }

      StringBuilder sb = new StringBuilder(txt);
      foreach (var match in matchesOrdered)
      {
        var snakeKey = match.Groups[1].Value;
        var upperKey = convertSnakeToUpper(snakeKey);
        sb.Replace(snakeKey, upperKey, match.Groups[1].Index, match.Groups[1].Value.Length);
      }

      string ret = sb.ToString();
      return ret;
    }

    private static async Task<byte[]> DownloadSpeechAsync(HttpClient httpClient, string url, string body)
    {
      var requestContent = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
      var response = httpClient.PostAsync(url, requestContent).GetAwaiter().GetResult();

      if (!response.IsSuccessStatusCode)
      {
        string errMsg;
        try
        {
          var jsonResponse = await response.Content.ReadAsStringAsync();
          var json = ESystem.Json.EJObject.Parse(jsonResponse);
          var detail = json["detail"].AsDict();
          var status = detail["status"].AsString();
          var msg = detail["message"].AsString();
          errMsg = $"Status: {status} // Message: {msg}";
        }
        catch (Exception e)
        {
          errMsg = $"Failed to obtain more details. : " + e.Message;
        }
        var ex = new TtsApplicationException(errMsg);
        var exex = new TtsApplicationException(
          $"POST request returned {response.StatusCode}:{response.Content}.", ex);
        var exexex = new TtsApplicationException(
          "Failed to download speech.", exex);
        throw exexex;
      }

      var ret = await Try(
        async () => await response.Content.ReadAsByteArrayAsync(),
        ex => new TtsApplicationException("Failed to read POST response stream.", ex));

      return ret;
    }

    private string BuildHttpGetModelJson(string text, double stability, double similarity_boost, double style)
    {
      string ret = Try(
        () => ConvertObjectToJson(new HttpGetModel(text, VoiceSettings: new VoiceSettings(similarity_boost, stability, style))),
        e => new TtsApplicationException($"Failed to create a model for text '{text}'.", e));
      return ret;
    }

    private string ConvertObjectToJson(HttpGetModel model)
    {
      string ret;
      try
      {
        var settings = new JsonSerializerSettings
        {
          ContractResolver = new DefaultContractResolver
          {
            NamingStrategy = new SnakeCaseNamingStrategy { ProcessDictionaryKeys = true }
          },
          Formatting = Formatting.Indented
        };
        ret = JsonConvert.SerializeObject(model, settings);
      }
      catch (Exception ex)
      {
        throw new TtsApplicationException($"Failed to convert {model.GetType().Name} to JSON.", ex);
      }
      return ret;
    }

    private static async Task<List<string>> DownloadModelsAsync(HttpClient httpClient, string url)
    {
      var response = await httpClient.GetAsync(url);

      if (response.StatusCode != System.Net.HttpStatusCode.OK)
        throw new TtsApplicationException("Failed to download models.",
          new ApplicationException($"GET request returned {response.StatusCode}:{response.Content}."));

      var tmp = await Try(
        async () => await response.Content.ReadAsStringAsync(),
        ex => new TtsApplicationException("Failed to read POST response stream.", ex));

      tmp = NormalizeSnakeCaseToUpperCase(tmp);

      List<string> ret = JsonConvert.DeserializeObject<List<string>>(tmp)!;

      return ret;
    }

    #endregion Methods
  }
}
