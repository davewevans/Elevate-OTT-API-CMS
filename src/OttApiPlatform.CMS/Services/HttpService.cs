namespace OttApiPlatform.CMS.Services;

public class HttpService : IHttpService
{
    #region Private Fields

    private readonly IHttpClientFactory _httpClientFactory;

    #endregion Private Fields

    #region Public Constructors

    public HttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    #endregion Public Constructors

    #region Private Properties

    private static JsonSerializerOptions DefaultJsonSerializerOptions => new()
    {
        PropertyNameCaseInsensitive = true
    };

    #endregion Private Properties

    #region Public Methods

    public async Task<ApiResponseWrapper<TResponse>> Get<TResponse>(string url)
    {
        using var httpClient = _httpClientFactory.CreateClient("DefaultClient");

        using var response = await httpClient.GetAsync(url).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> Get<TRequest, TResponse>(string url, TRequest data)
    {
        var dataJson = JsonSerializer.Serialize(data);

        var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url),
            Content = stringContent,
        };

        using var httpClient = _httpClientFactory.CreateClient("DefaultClient");

        using var response = await httpClient.SendAsync(request).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> Post<TRequest, TResponse>(string url, TRequest data)
    {
        using var httpClient = _httpClientFactory.CreateClient("DefaultClient");

        var dataJson = JsonSerializer.Serialize(data);

        var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

        using var response = await httpClient.PostAsync(url, stringContent).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> Post<TResponse>(string url)
    {
        using var httpClient = _httpClientFactory.CreateClient("DefaultClient");

        using var response = await httpClient.PostAsync(url, null).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> PostFormData<TRequest, TResponse>(string url, MultipartFormDataContent data, CancellationToken cancellationToken = default)
    {
        using var httpClient = _httpClientFactory.CreateClient("DefaultClient");

        using var response = await httpClient.PostAsync(url, data, cancellationToken: cancellationToken).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> Put<TRequest, TResponse>(string url, TRequest data)
    {
        using var httpClient = _httpClientFactory.CreateClient("DefaultClient");

        var dataJson = JsonSerializer.Serialize(data);

        var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

        using var response = await httpClient.PutAsync(url, stringContent);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> PutFormData<TRequest, TResponse>(string url, MultipartFormDataContent data, CancellationToken cancellationToken = default)
    {
        using var httpClient = _httpClientFactory.CreateClient("DefaultClient");

        using var response = await httpClient.PutAsync(url, data, cancellationToken: cancellationToken).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> PutRawContent<TResponse>(string url, HttpContent content, CancellationToken cancellationToken = default)
    {
        using var httpClient = _httpClientFactory.CreateClient("NoBaseUrlClient");
        httpClient.DefaultRequestHeaders.Authorization = null;
        using var response = await httpClient.PutAsync(url, content, cancellationToken).ConfigureAwait(false);

        return await ProcessNoResponseBody<TResponse>(response);
    }


    public async Task<ApiResponseWrapper<TResponse>> Delete<TResponse>(string url)
    {
        using var httpClient = _httpClientFactory.CreateClient("DefaultClient");

        using var response = await httpClient.DeleteAsync(url);

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

    private async Task<ApiResponseWrapper<TResponse>> ProcessNoResponseBody<TResponse>(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}. Response: {responseContent}");
        }

        if (string.IsNullOrWhiteSpace(responseContent))
        {
            return new ApiResponseWrapper<TResponse>(null, response.IsSuccessStatusCode, response.StatusCode);
        }

        return await ProcessResponse<TResponse>(response);
    }


    #endregion Private Methods
}