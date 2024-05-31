using System;
using Manero_Customer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using Xunit;

namespace ManeroCustomerTest;

public class CookieServiceTest
{
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly CookieService _cookieService;
    private readonly Mock<HttpContext> _mockHttpContext;
    private readonly Mock<HttpRequest> _mockHttpRequest;
    private readonly Mock<HttpResponse> _mockHttpResponse;
    private readonly Dictionary<string, string> _requestCookies;
    private readonly Dictionary<string, string> _responseCookies;

    public CookieServiceTest()
    {
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockHttpContext = new Mock<HttpContext>();
        _mockHttpRequest = new Mock<HttpRequest>();
        _mockHttpResponse = new Mock<HttpResponse>();
        _requestCookies = new Dictionary<string, string>();
        _responseCookies = new Dictionary<string, string>();

        _mockHttpRequest.Setup(r => r.Cookies).Returns(new MockRequestCookieCollection(_requestCookies));
        _mockHttpResponse.Setup(r => r.Cookies).Returns(new MockResponseCookies(_responseCookies));

        _mockHttpContext.Setup(c => c.Request).Returns(_mockHttpRequest.Object);
        _mockHttpContext.Setup(c => c.Response).Returns(_mockHttpResponse.Object);
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(_mockHttpContext.Object);

        _cookieService = new CookieService(_mockHttpContextAccessor.Object);
    }

    [Fact]
    public void SetSessionIdCookie_SetsCookieWithCorrectValue()
    {
        // Arrange
        var sessionId = "test-session-id";

        // Act
        _cookieService.SetSessionIdCookie(sessionId);

        // Assert
        Assert.Equal(sessionId, _responseCookies["SessionId"]);
    }

    [Fact]
    public void GetSessionIdCookie_ReturnsCorrectValue_WhenCookieExists()
    {
        // Arrange
        var sessionId = "test-session-id";
        _requestCookies["SessionId"] = sessionId;

        // Act
        var result = _cookieService.GetSessionIdCookie();

        // Assert
        Assert.Equal(sessionId, result);
    }

    [Fact]
    public void GetSessionIdCookie_ReturnsNull_WhenCookieDoesNotExist()
    {
        // Arrange

        // Act
        var result = _cookieService.GetSessionIdCookie();

        // Assert
        Assert.Null(result);
    }

    private class MockRequestCookieCollection : IRequestCookieCollection
    {
        private readonly Dictionary<string, string> _cookies;

        public MockRequestCookieCollection(Dictionary<string, string> cookies)
        {
            _cookies = cookies;
        }

        public string this[string key] => _cookies.ContainsKey(key) ? _cookies[key] : null;

        public int Count => _cookies.Count;

        public ICollection<string> Keys => _cookies.Keys;

        public bool ContainsKey(string key)
        {
            return _cookies.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _cookies.GetEnumerator();
        }

        public bool TryGetValue(string key, out string value)
        {
            return _cookies.TryGetValue(key, out value);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _cookies.GetEnumerator();
        }
    }

    private class MockResponseCookies : IResponseCookies
    {
        private readonly Dictionary<string, string> _cookies;

        public MockResponseCookies(Dictionary<string, string> cookies)
        {
            _cookies = cookies;
        }

        public void Append(string key, string value)
        {
            _cookies[key] = value;
        }

        public void Append(string key, string value, CookieOptions options)
        {
            _cookies[key] = value;
        }

        public void Delete(string key)
        {
            _cookies.Remove(key);
        }

        public void Delete(string key, CookieOptions options)
        {
            _cookies.Remove(key);
        }
    }
}

