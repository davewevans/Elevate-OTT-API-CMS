namespace OttApiPlatform.Gateway.Services;

public class HttpService : IHttpService
{
    #region Private Fields

    private readonly HttpClient _httpClient;

    #endregion Private Fields

    #region Public Constructors

    public HttpService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    #endregion Public Constructors

    #region Public Properties

    public TenantHeader TenantHeader { get; private set; } = new();

    #endregion Public Properties

    #region Private Properties

    private static JsonSerializerOptions DefaultJsonSerializerOptions => new()
    {
        PropertyNameCaseInsensitive = true
    };

    #endregion Private Properties

    #region Public Methods

    public void SetTenantHeader(string value)
    {
        TenantHeader = new TenantHeader() { Key = "Bp-TenantByGatewayClient", Value = value };
    }

    public async Task<ApiResponseWrapper<TResponse>> Get<TResponse>(string url)
    {
        _httpClient.DefaultRequestHeaders.Clear();

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", CultureInfo.CurrentCulture.ToString());

        using var response = await _httpClient.GetAsync(url);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> Get<TRequest, TResponse>(string url, TRequest data)
    {
        _httpClient.DefaultRequestHeaders.Clear();

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", CultureInfo.CurrentCulture.ToString());

        var dataJson = JsonSerializer.Serialize(data);

        var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url),
            Content = stringContent,
        };

        using var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> Post<TRequest, TResponse>(string url, TRequest data)
    {
        _httpClient.DefaultRequestHeaders.Clear();

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", CultureInfo.CurrentCulture.ToString());

        _httpClient.DefaultRequestHeaders.Add(TenantHeader.Key, TenantHeader.Value);

        var dataJson = JsonSerializer.Serialize(data);

        var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

        using var response = await _httpClient.PostAsync(url, stringContent);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> Post<TResponse>(string url)
    {
        _httpClient.DefaultRequestHeaders.Clear();

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", CultureInfo.CurrentCulture.ToString());

        using var response = await _httpClient.PostAsync(url, null);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> PostFormData<TRequest, TResponse>(string url, MultipartFormDataContent data, CancellationToken cancellationToken = default)
    {
        _httpClient.DefaultRequestHeaders.Clear();

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", CultureInfo.CurrentCulture.ToString());

        using var response = await _httpClient.PostAsync(url, data, cancellationToken: cancellationToken).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> Put<TRequest, TResponse>(string url, TRequest data)
    {
        _httpClient.DefaultRequestHeaders.Clear();

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", CultureInfo.CurrentCulture.ToString());

        var dataJson = JsonSerializer.Serialize(data);

        var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

        using var response = await _httpClient.PutAsync(url, stringContent);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> PutFormData<TRequest, TResponse>(string url, MultipartFormDataContent data, CancellationToken cancellationToken = default)
    {
        _httpClient.DefaultRequestHeaders.Clear();

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", CultureInfo.CurrentCulture.ToString());

        using var response = await _httpClient.PutAsync(url, data, cancellationToken: cancellationToken).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> Delete<TResponse>(string url)
    {
        _httpClient.DefaultRequestHeaders.Clear();

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", CultureInfo.CurrentCulture.ToString());

        using var response = await _httpClient.DeleteAsync(url);

        return await ProcessResponse<TResponse>(response);
    }

    #endregion Public Methods

    #region Private Methods

    private static async Task<ApiResponseWrapper<TResponse>> ProcessResponse<TResponse>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            var responseDeserialized = await response.Content.ReadFromJsonAsync<SuccessResult<TResponse>>(DefaultJsonSerializerOptions).ConfigureAwait(false);

            if (responseDeserialized != null)
                return new ApiResponseWrapper<TResponse>(responseDeserialized.Payload, response.IsSuccessStatusCode, response.StatusCode);

            throw new ArgumentNullException($"ProcessResponse: {nameof(responseDeserialized)} cannot be NULL.");
        }
        else
        {
            var responseDeserialized = await response.Content.ReadFromJsonAsync<ApiErrorResponse>(DefaultJsonSerializerOptions).ConfigureAwait(false);

            return new ApiResponseWrapper<TResponse>(responseDeserialized, response.IsSuccessStatusCode, response.StatusCode);
        }
    }

    #endregion Private Methods
}