using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormRecognizer
{


    public class AnalyzeResultResponse
    {
        public string status { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime lastUpdatedDateTime { get; set; }
        public Analyzeresult analyzeResult { get; set; }
    }

    public class Analyzeresult
    {
        public string version { get; set; }
        public Readresult[] readResults { get; set; }
        public Pageresult[] pageResults { get; set; }
        public Documentresult[] documentResults { get; set; }
        public object[] errors { get; set; }
    }

    public class Readresult
    {
        public int page { get; set; }
        public string language { get; set; }
        public float angle { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string unit { get; set; }
    }

    public class Pageresult
    {
        public int page { get; set; }
        public object[] tables { get; set; }
    }

    public class Documentresult
    {
        public string docType { get; set; }
        public int[] pageRange { get; set; }
        public Fields fields { get; set; }
    }

    public class Fields
    {
        public Sex Sex { get; set; }
        public DOB DOB { get; set; }
        public Placeofissue PlaceOfIssue { get; set; }
        public Names Names { get; set; }
        public Nidnumber NidNumber { get; set; }
    }

    public class FieldBase
    {
        public string type { get; set; }
        public string valueString { get; set; }
        public string text { get; set; }
        public int page { get; set; }
        public float[] boundingBox { get; set; }
        public float confidence { get; set; }
    }

    public class Sex : FieldBase
    {

    }

    public class DOB : FieldBase
    {

    }

    public class Placeofissue : FieldBase
    {

    }

    public class Names : FieldBase
    {

    }

    public class Nidnumber : FieldBase
    {

    }
}
