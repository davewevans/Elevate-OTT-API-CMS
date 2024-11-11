namespace OttApiPlatform.Application.Common.Contracts;

public interface IServiceUtils
{
    /// <summary>
    /// Verifies a streaming url is HLS streaming.
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    bool VerifyIsHls(string url);

    /// <summary>
    /// Convert Unix time value to a DateTime object.
    /// </summary>
    /// <param name="unixtime">The Unix time stamp you want to convert to DateTime.</param>
    /// <returns>Returns a DateTime object that represents value of the Unix time.</returns>
    DateTime UnixTimeToDateTime(long unixtime);

    /// <summary>
    /// Convert a DateTime to a unix timestamp
    /// </summary>
    /// <param name="MyDateTime">The DateTime object to convert into a Unix Time</param>
    /// <returns></returns>
    long DateTimeToUnix(DateTime MyDateTime);

    string EncodeTo64(string toEncode);

    string DecodeBase64(string toDecode);
}
