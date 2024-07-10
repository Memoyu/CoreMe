namespace CoreMe.Domain.Constants;
public static class CacheKeyConst
{
    public static string ArticleView(long articleId, long visitorId) => $"memo-blog:article:views:{articleId}:{visitorId}";
}
