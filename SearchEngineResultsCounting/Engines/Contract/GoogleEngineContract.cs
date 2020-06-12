using System.Collections.Generic;

namespace SearchEngineResultsCounting.Engines.Contract
{
    public class GoogleEngineContract
    {
        public class Url
        {
            public string type { get; set; }
            public string template { get; set; }
        }

        public class Request
        {
            public string title { get; set; }
            public string totalResults { get; set; }
            public string searchTerms { get; set; }
            public int count { get; set; }
            public int startIndex { get; set; }
            public string inputEncoding { get; set; }
            public string outputEncoding { get; set; }
            public string safe { get; set; }
            public string cx { get; set; }
        }

        public class NextPage
        {
            public string title { get; set; }
            public string totalResults { get; set; }
            public string searchTerms { get; set; }
            public int count { get; set; }
            public int startIndex { get; set; }
            public string inputEncoding { get; set; }
            public string outputEncoding { get; set; }
            public string safe { get; set; }
            public string cx { get; set; }
        }

        public class Queries
        {
            public IList<Request> request { get; set; }
            public IList<NextPage> nextPage { get; set; }
        }

        public class Context
        {
            public string title { get; set; }
            public IList<IList<Face>> facets { get; set; }
        }
         
        public class Face
        {
          public string anchor { get; set; }
          public string label { get; set; }
          public string label_with_op {get; set;}
        }

        public class SearchInformation
        {
            public double searchTime { get; set; }
            public string formattedSearchTime { get; set; }
            public string totalResults { get; set; }
            public string formattedTotalResults { get; set; }
        }

        public class Label
        {
            public string name { get; set; }
            public string displayName { get; set; }
            public string label_with_op { get; set; }
        }

        public class Metatag
        {
            public string originator { get; set; }
            public string progid { get; set; }
            public string title { get; set; }
            public string created { get; set; }
            public string changedby { get; set; }
            public string changed { get; set; }
            public string author { get; set; }
        }

        public class CseThumbnail
        {
            public string src { get; set; }
            public string width { get; set; }
            public string height { get; set; }
        }

        public class CseImage
        {
            public string src { get; set; }
        }

        public class Pagemap
        {
            public IList<Metatag> metatags { get; set; }
            public IList<CseThumbnail> cse_thumbnail { get; set; }
            public IList<CseImage> cse_image { get; set; }
        }

        public class Item
        {
            public string kind { get; set; }
            public string title { get; set; }
            public string htmlTitle { get; set; }
            public string link { get; set; }
            public string displayLink { get; set; }
            public string snippet { get; set; }
            public string htmlSnippet { get; set; }
            public string cacheId { get; set; }
            public string formattedUrl { get; set; }
            public string htmlFormattedUrl { get; set; }
            public IList<Label> labels { get; set; }
            public Pagemap pagemap { get; set; }
            public string mime { get; set; }
            public string fileFormat { get; set; }
        }

        public class Root
        {
            public string kind { get; set; }
            public Url url { get; set; }
            public Queries queries { get; set; }
            public Context context { get; set; }
            public SearchInformation searchInformation { get; set; }
            public IList<Item> items { get; set; }
        }

    }
}