using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIDRetrieve.Model
{
    public class dataResponse
    {
        public string status { get; set; }
        public string statusCode { get; set; }
        public successValue success { get; set; }
    }
    public class successValue
    {
        public dataValue data { get; set; }
    }
    public class dataValue
    {
        public string requestId { get; set; }
        public string name { get; set; }
        public string nameEn { get; set; }
        public string gender { get; set; }
        public string bloodGroup { get; set; }
        public string dateOfBirth { get; set; }
        public string father { get; set; }
        public string mother { get; set; }
        public string spouse { get; set; }
        public string nationalId { get; set; }
        public string pin { get; set; }
        public permanentAddress permanentAddress { get; set; }
        public string photo { get; set; }
        public presentAddress presentAddress { get; set; }
        public string formNo { get; set; }
        public string voterArea { get; set; }
        public string voterAreaCode { get; set; }
        public string voterNo { get; set; }
        public string birthPlace { get; set; }
        public string education { get; set; }
    }
    public class permanentAddress
    {
        public string division { get; set; }
        public string district { get; set; }
        public string rmo { get; set; }
        public string cityCorporationOrMunicipality { get; set; }
        public string mouzaOrMoholla { get; set; }
        public string villageOrRoad { get; set; }
        public string upozila { get; set; }
        public string unionOrWard { get; set; }
        public string postOffice { get; set; }
        public string postalCode { get; set; }
        public string wardForUnionPorishod { get; set; }
        public string additionalMouzaOrMoholla { get; set; }
        public string additionalVillageOrRoad { get; set; }
        public string homeOrHoldingNo { get; set; }
        public string region { get; set; }

    }

    public class presentAddress
    {
        public string division { get; set; }
        public string district { get; set; }
        public string rmo { get; set; }
        public string cityCorporationOrMunicipality { get; set; }
        public string mouzaOrMoholla { get; set; }
        public string villageOrRoad { get; set; }
        public string upozila { get; set; }
        public string unionOrWard { get; set; }
        public string postOffice { get; set; }
        public string postalCode { get; set; }
        public string wardForUnionPorishod { get; set; }
        public string additionalMouzaOrMoholla { get; set; }
        public string additionalVillageOrRoad { get; set; }
        public string homeOrHoldingNo { get; set; }
        public string region { get; set; }
    }


    public class ApiUserInfo
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class ApiAccessToken
    {
        public string status { get; set; }
        public string statusCode { get; set; }
        public success success { get; set; }
    }

    public class success
    {
        public data data { get; set; }
    }
    public class data
    {
        public string username { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }

    }

    public class nidrequest
    {
        //public string dateOfBirth { get; set; }
        public string nid10Digit { get; set; }

    }
}
