﻿namespace OttApiPlatform.CMS.Contracts.Services;

public interface IHttpService
{
    #region Public Methods

    Task<ApiResponseWrapper<TResponse>> Get<TResponse>(string url);

    Task<ApiResponseWrapper<TResponse>> Get<TRequest, TResponse>(string url, TRequest data);

    Task<ApiResponseWrapper<TResponse>> Post<TRequest, TResponse>(string url, TRequest data);

    Task<ApiResponseWrapper<TResponse>> Post<TResponse>(string url);

    Task<ApiResponseWrapper<TResponse>> PostFormData<TRequest, TResponse>(string url, MultipartFormDataContent data, CancellationToken cancellationToken = default);

    Task<ApiResponseWrapper<TResponse>> Put<TRequest, TResponse>(string url, TRequest data);

    Task<ApiResponseWrapper<TResponse>> PutFormData<TRequest, TResponse>(string url, MultipartFormDataContent data, CancellationToken cancellationToken = default);

    Task<ApiResponseWrapper<TResponse>> PutRawContent<TResponse>(string url, HttpContent content,
        CancellationToken cancellationToken = default);
    Task<ApiResponseWrapper<TResponse>> Delete<TResponse>(string url);

    #endregion Public Methods
}