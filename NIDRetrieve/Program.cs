using System;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using OfficeOpenXml;
using System.Threading;



using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics.Metrics;
using System.Xml.Linq;
using System.Drawing;
using NIDRetrieve.Model;
using static System.Net.Mime.MediaTypeNames;
using System.Dynamic;
using System.IO;
using System.Diagnostics;
using static System.Net.WebRequestMethods;
using System.Net;

namespace NIDRetrieve
{
    internal class Program
    {
        /*public string nid { get; set; }
        public string pin { get; set; }
        public string name { get; set; }
        public string nameEn { get; set; }
        public string gender { get; set; }
        public string bloodGroup { get; set; }
        public string dateOfBirth { get; set; }
        public string fatherName { get; set; }
        public string motherName { get; set; }
        public string spouseName { get; set; }
        public string perDivision { get; set; }
        public string perDistrict { get; set; }
        public string perRMO { get; set; }
        public string perCityCorporationOrMunicipality { get; set; }
        public string perMouzaOrMoholla { get; set; }
        public string perVillageOrRoad { get; set; }
        public string perUpazila { get; set; }
        public string perUnionOrWard { get; set; }
        public string perPostOffice { get; set; }
        public string perPostalCode { get; set; }
        public string perWardForUnionPorishod { get; set; }
        public string perAdditionalMouzaOrMoholla { get; set; }
        public string perAdditionalVillageOrRoad { get; set; }
        public string perHomeOrHoldingNo { get; set; }
        public string perRegion { get; set; }
        public string photoUrl { get; set; }
        public string prDivision { get; set; }
        public string prDistrict { get; set; }
        public string prRMO { get; set; }
        public string prCityCorporationOrMunicipality { get; set; }
        public string prMouzaOrMoholla { get; set; }
        public string prVillageOrRoad { get; set; }
        public string prUpazila { get; set; }
        public string prUnionOrWard { get; set; }
        public string prPostOffice { get; set; }
        public string prPostalCode { get; set; }
        public string prWardForUnionPorishod { get; set; }
        public string prAdditionalMouzaOrMoholla { get; set; }
        public string prAdditionalVillageOrRoad { get; set; }
        public string prHomeOrHoldingNo { get; set; }
        public string prRegion { get; set; }
        public string formNumber { get; set; }
        public string voterArea { get; set; }
        public string voterAreaCode { get; set; }
        public string voterNumber { get; set; }
        public string birthPlace { get; set; }
        public string education { get; set; }
        //public string Id { get; set; }
        //public string Name { get; set; }
        //public string Country { get; set; }*/
        public string nid { get; set; }
        public string pin { get; set; }
        public string name { get; set; }
        public string nameEn { get; set; }
        public string gender { get; set; }
        public string bloodGroup { get; set; }
        public string dateOfBirth { get; set; }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            OnPost();
        }

        private static void OnPost(/*string nid, string dateOfBirth*/)
        {
            string Baseurl = "https://prportal.nidw.gov.bd";
            // https://prportal.nidw.gov.bd/partner-service/rest/voter/details

            try
            {
                //List<Employee> EmpInfo = new List<Employee>();
                using (var client = new HttpClient())
                {
                    //Passing service base url

                    //Need to replace userName and Password of actual API
                    var userInfo = new ApiUserInfo()
                    {
                        username = "partner",
                        password = "Ecs@123456"
                    };

                    client.BaseAddress = new Uri("https://prportal.nidw.gov.bd");

                    StringContent body = new StringContent(JsonConvert.SerializeObject(userInfo), Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.PostAsync("/partner-service/rest/auth/login", body);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var content = response.Result.Content.ReadAsStringAsync();
                        ApiAccessToken objResultApiAccessToken = JsonConvert.DeserializeObject<ApiAccessToken>(content.Result);

                        using (var client2 = new HttpClient())
                        {
                            client2.BaseAddress = new Uri("https://prportal.nidw.gov.bd");
                            client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client2.DefaultRequestHeaders.Add("Authorization", "Bearer " + objResultApiAccessToken.success.data.access_token);
                            //Excel File
                            string excelFilePath = @"D:/Mahbub Edit_NID.xlsx";
                            //EPP Package
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                            using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
                            {
                                // Get the worksheet named "10" in the Excel file
                                var worksheet = package.Workbook.Worksheets["10"];

                                for (int i = 1; i <= 5; i++)
                                {
                                    int rowIndex = i; // Row index (1-based)
                                    int columnIndex = 2; // Column index (1-based)

                                    // Read the value from the specified cell
                                    object cellValue = worksheet.Cells[rowIndex, columnIndex].Value;
                                    string nid = cellValue.ToString(); //Convert.ToString(cellValue);
                                    nid = nid.Trim();
                                    nidrequest nidrequestObj = new nidrequest()
                                    {
                                        //dateOfBirth = dateOfBirth,
                                        nid10Digit = nid
                                        //dateOfBirth = "1988-03-24", 
                                        //nid10Digit = "5507500006"
                                    };

                                    StringContent body2 = new StringContent(JsonConvert.SerializeObject(nidrequestObj), Encoding.UTF8, "application/json");


                                    var nidResponse = client2.PostAsync("/partner-service/rest/voter/details", body2);

                                    if (nidResponse.Result.IsSuccessStatusCode)
                                    {
                                        var NidContent = nidResponse.Result.Content.ReadAsStringAsync();

                                        dataResponse nidDataResponse = JsonConvert.DeserializeObject<dataResponse>(NidContent.Result);
                                        /***this.nid = nidDataResponse.success.data.nationalId;
                                        this.pin = nidDataResponse.success.data.pin;
                                        this.name = nidDataResponse.success.data.name;
                                        this.nameEn = nidDataResponse.success.data.nameEn;
                                        this.gender = nidDataResponse.success.data.gender;
                                        this.bloodGroup = nidDataResponse.success.data.bloodGroup;
                                        this.dateOfBirth = dateOfBirth;***/

                                        //string browser = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
                                        
                                        string target = nidDataResponse.success.data.photo;


                                        /***************Downloading Image using Webclient***************/
                                        string fileName = "D:\\Images\\image.jpg";
                                        // Create a new WebClient instance.
                                        WebClient myWebClient = new WebClient();

                                        // Download the Web resource and save it into the current filesystem folder.
                                        myWebClient.DownloadFile(target, fileName);
                                        Console.WriteLine("Image Download Complete...");
                                        Thread.Sleep(6000);


                                        /***************Downloading Image using Browser***************/
                                        /***string browser = @"C:\Program Files (x86)\Microsoft\EdgeCore\123.0.2420.65\msedge.exe";
                                        //string target =  "https://prportal.nidw.gov.bd/file-9d/b/e/1/675f8ac2-9ee2-4709-8968-376e92678716/Photo-675f8ac2-9ee2-4709-8968-376e92678716.jpg?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=fileobj%2F20231219%2Fus-east-1%2Fs3%2Faws4_request&X-Amz-Date=20231219T111730Z&X-Amz-Expires=120&X-Amz-SignedHeaders=host&X-Amz-Signature=0fe69a2d6d936a7d9e382a478f3c49a26ec1b59c89fe7756e16551427d732857";
                                        Process.Start(browser, target);
                                        Thread.Sleep(6000);
                                        Process[] Edge = Process.GetProcessesByName("msedge");

                                        foreach (Process Item in Edge)
                                        {
                                            try
                                            {
                                                Item.Kill();
                                                Item.WaitForExit(100);
                                            }
                                            catch (Exception)
                                            {

                                            }
                                        }***/

                                        InsertNid(nidDataResponse);
                                        Console.WriteLine("Number " + i.ToString() + " Complete", i);
                                        Thread.Sleep(15000);
                                        //return nidDataResponse;

                                        //HttpContext.Response.Body.Write(nidDataResponse.success.data.name);
                                        //HttpContext.Response.Body.Write(objResultApiAccessToken.success.data.access_token);


                                        // var responseObject =  response.Result.Content.ReadAsAsync<ApiAccessToken>();

                                        // var objResultApiAccessToken = JsonConvert.DeserializeObject(content.Result);

                                        // ApiAccessToken objResultApiAccessToken = JsonConvert.DeserializeObject<ApiAccessToken>(content.Result);
                                    }
                                    
                                    

                                    
                                    //Task.Delay(15000);
                                    
                                    
                                    
                                }
                            }
                        }
                    }
                }
                //return new dataResponse { };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
                //throw;
            }
            //dataResponse dResponse = NidInfoRetrieve(nid, dateOfBirth);
        }
        private static void InsertNid(dataResponse nidDataResponse/*
           string nid, string pin, string name, string nameEn, string gender,
           string bloodGroup, string dateOfBirth, string fatherName, string motherName,
           string spouseName, string perDivision, string perDistrict,
           string perRMO, string perCityCorporationOrMunicipality,
           string perMouzaOrMoholla, string perVillageOrRoad, string perUpazila,
           string perUnionOrWard, string perPostOffice, string perPostalCode,
           string perWardForUnionPorishod, string perAdditionalMouzaOrMoholla,
           string perAdditionalVillageOrRoad, string perHomeOrHoldingNo,
           string perRegion, string photoUrl, string prDivision, string prDistrict,
           string prRMO, string prCityCorporationOrMunicipality,
           string prMouzaOrMoholla, string prVillageOrRoad,
           string prUpazila, string prUnionOrWard, string prPostOffice,
           string prPostalCode, string prWardForUnionPorishod, string prAdditionalMouzaOrMoholla,
           string prAdditionalVillageOrRoad, string prHomeOrHoldingNo, string prRegion,
           string formNumber, string voterArea, string voterAreaCode,
           string voterNumber, string birthPlace, string education*/)
        {
            /*Id = id;
            Name = name;
            Country = country;
            */

            //string nName = Request.Form["textbox"];


            //string cString = "Data Source=(localdb)\\MSSQLLocalDB;Database=Company;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string cString="Data Source = (localdb)\\MSSQLLocalDB; Database = Voter; Integrated Security = True; Connect Timeout = 30; Encrypt = False";
            
            //string cString = configuration["ConnectionStrings:DefaultConnection2"];

            //string insQuery = "INSERT INTO Person (Name) VALUES(@Name)";
            string insQuery = "INSERT INTO VoterDetails (nid, pin, name, nameEn, gender,bloodGroup," +
            "dateOfBirth,fatherName,motherName,spouseName," +
            "perDivision,perDistrict,perRMO,perCityCorporationOrMunicipality," +
                "perMouzaOrMoholla,perVillageOrRoad,perUpazila," +
                "perUnionOrWard,perPostOffice,perPostalCode," +
                "perWardForUnionPorishod,perAdditionalMouzaOrMoholla," +
                "perAdditionalVillageOrRoad,perHomeOrHoldingNo," +
                "perRegion,photoUrl,prDivision,prDistrict," +
                "prRMO,prCityCorporationOrMunicipality," +
                "prMouzaOrMoholla,prVillageOrRoad,prUpazila," +
                "prUnionOrWard,prPostOffice,prPostalCode," +
                "prWardForUnionPorishod,prAdditionalMouzaOrMoholla," +
                "prAdditionalVillageOrRoad,prHomeOrHoldingNo," +
                "prRegion,formNumber,voterArea,voterAreaCode," +
                "voterNumber,birthPlace,education,photo)" +
                "VALUES(" +
                "@nid, @pin, @name, @nameEn, @gender,@bloodGroup," +
                "@dateOfBirth,@fatherName,@motherName,@spouseName," +
                "@perDivision,@perDistrict,@perRMO,@perCityCorporationOrMunicipality," +
                "@perMouzaOrMoholla,@perVillageOrRoad,@perUpazila," +
                "@perUnionOrWard,@perPostOffice,@perPostalCode," +
                "@perWardForUnionPorishod,@perAdditionalMouzaOrMoholla," +
                "@perAdditionalVillageOrRoad,@perHomeOrHoldingNo," +
                "@perRegion,@photoUrl,@prDivision,@prDistrict," +
                "@prRMO,@prCityCorporationOrMunicipality," +
                "@prMouzaOrMoholla,@prVillageOrRoad,@prUpazila," +
                "@prUnionOrWard,@prPostOffice,@prPostalCode," +
                "@prWardForUnionPorishod,@prAdditionalMouzaOrMoholla," +
                "@prAdditionalVillageOrRoad,@prHomeOrHoldingNo," +
                "@prRegion,@formNumber,@voterArea,@voterAreaCode," +
                "@voterNumber,@birthPlace,@education,@photo)";



            //string rdQuery = "SELECT * FROM Person";

            //DateTime Dob = Convert.ToDateTime(dateOfBirth);
            DateTime Dob = Convert.ToDateTime(nidDataResponse.success.data.dateOfBirth);
            
            //string photoUrl = GetPhotoUrl();

            string photoUrl= "D:\\Images\\image.jpg";
            byte[] data = new byte[photoUrl.Length];
            FileInfo fileInfo = new FileInfo(photoUrl);

            // Load a filestream and put its content into the byte[]
            using (FileStream fs = fileInfo.OpenRead())
            {
                fs.Read(data, 0, data.Length);
            }

            using (SqlConnection conn = new SqlConnection(cString))
            {
                SqlCommand insCmd = new SqlCommand(insQuery, conn);

                

                insCmd.Parameters.AddWithValue("@nid", (nidDataResponse.success.data.nationalId == null ? "" : nidDataResponse.success.data.nationalId));
                insCmd.Parameters.AddWithValue("@pin", (nidDataResponse.success.data.pin == null ? "" : nidDataResponse.success.data.pin));
                insCmd.Parameters.AddWithValue("@name", (nidDataResponse.success.data.name == null ? "" : nidDataResponse.success.data.name));
                insCmd.Parameters.AddWithValue("@nameEn", (nidDataResponse.success.data.nameEn == null ? "" : nidDataResponse.success.data.nameEn));
                insCmd.Parameters.AddWithValue("@gender", (nidDataResponse.success.data.gender == null ? "" : nidDataResponse.success.data.gender));
                insCmd.Parameters.AddWithValue("@bloodGroup", (nidDataResponse.success.data.bloodGroup == null ? "" : nidDataResponse.success.data.bloodGroup));
                insCmd.Parameters.AddWithValue("@dateOfBirth", Dob);
                insCmd.Parameters.AddWithValue("@fatherName", (nidDataResponse.success.data.father == null ? "" : nidDataResponse.success.data.father));
                insCmd.Parameters.AddWithValue("@motherName", (nidDataResponse.success.data.mother == null ? "" : nidDataResponse.success.data.mother));
                insCmd.Parameters.AddWithValue("@spouseName", (nidDataResponse.success.data.spouse == null ? "" : nidDataResponse.success.data.spouse));
                insCmd.Parameters.AddWithValue("@perDivision", (nidDataResponse.success.data.permanentAddress.division == null ? "" : nidDataResponse.success.data.permanentAddress.division));
                insCmd.Parameters.AddWithValue("@perDistrict", (nidDataResponse.success.data.permanentAddress.district == null ? "" : nidDataResponse.success.data.permanentAddress.district));
                insCmd.Parameters.AddWithValue("@perRMO", (nidDataResponse.success.data.permanentAddress.rmo == null ? "" : nidDataResponse.success.data.permanentAddress.rmo));
                insCmd.Parameters.AddWithValue("@perCityCorporationOrMunicipality", (nidDataResponse.success.data.permanentAddress.cityCorporationOrMunicipality == null ? "" : nidDataResponse.success.data.permanentAddress.cityCorporationOrMunicipality));
                insCmd.Parameters.AddWithValue("@perMouzaOrMoholla", (nidDataResponse.success.data.permanentAddress.mouzaOrMoholla == null ? "" : nidDataResponse.success.data.permanentAddress.mouzaOrMoholla));
                insCmd.Parameters.AddWithValue("@perVillageOrRoad", (nidDataResponse.success.data.permanentAddress.villageOrRoad == null ? "" : nidDataResponse.success.data.permanentAddress.villageOrRoad));
                insCmd.Parameters.AddWithValue("@perUpazila", (nidDataResponse.success.data.permanentAddress.upozila == null ? "" : nidDataResponse.success.data.permanentAddress.upozila));
                insCmd.Parameters.AddWithValue("@perUnionOrWard", (nidDataResponse.success.data.permanentAddress.unionOrWard == null ? "" : nidDataResponse.success.data.permanentAddress.unionOrWard));
                insCmd.Parameters.AddWithValue("@perPostOffice", (nidDataResponse.success.data.permanentAddress.postOffice == null ? "" : nidDataResponse.success.data.permanentAddress.postOffice));
                insCmd.Parameters.AddWithValue("@perPostalCode", (nidDataResponse.success.data.permanentAddress.postalCode == null ? "" : nidDataResponse.success.data.permanentAddress.postalCode));
                insCmd.Parameters.AddWithValue("@perWardForUnionPorishod", (nidDataResponse.success.data.permanentAddress.wardForUnionPorishod == null ? "" : nidDataResponse.success.data.permanentAddress.wardForUnionPorishod));
                insCmd.Parameters.AddWithValue("@perAdditionalMouzaOrMoholla", (nidDataResponse.success.data.permanentAddress.additionalMouzaOrMoholla == null ? "" : nidDataResponse.success.data.permanentAddress.additionalMouzaOrMoholla));
                insCmd.Parameters.AddWithValue("@perAdditionalVillageOrRoad", (nidDataResponse.success.data.permanentAddress.additionalVillageOrRoad == null ? "" : nidDataResponse.success.data.permanentAddress.additionalVillageOrRoad));
                insCmd.Parameters.AddWithValue("@perHomeOrHoldingNo", (nidDataResponse.success.data.permanentAddress.homeOrHoldingNo == null ? "" : nidDataResponse.success.data.permanentAddress.homeOrHoldingNo));
                insCmd.Parameters.AddWithValue("@perRegion", (nidDataResponse.success.data.permanentAddress.region == null ? "" : nidDataResponse.success.data.permanentAddress.region));
                insCmd.Parameters.AddWithValue("@photoUrl", (nidDataResponse.success.data.photo == null ? "" : nidDataResponse.success.data.photo));
                insCmd.Parameters.AddWithValue("@prDivision", (nidDataResponse.success.data.presentAddress.division == null ? "" : nidDataResponse.success.data.presentAddress.division));
                insCmd.Parameters.AddWithValue("@prDistrict", (nidDataResponse.success.data.presentAddress.district == null ? "" : nidDataResponse.success.data.presentAddress.district));
                insCmd.Parameters.AddWithValue("@prRMO", (nidDataResponse.success.data.presentAddress.rmo == null ? "" : nidDataResponse.success.data.presentAddress.rmo));
                insCmd.Parameters.AddWithValue("@prCityCorporationOrMunicipality", (nidDataResponse.success.data.presentAddress.cityCorporationOrMunicipality == null ? "" : nidDataResponse.success.data.presentAddress.cityCorporationOrMunicipality));
                insCmd.Parameters.AddWithValue("@prMouzaOrMoholla", (nidDataResponse.success.data.presentAddress.mouzaOrMoholla == null ? "" : nidDataResponse.success.data.presentAddress.mouzaOrMoholla));
                insCmd.Parameters.AddWithValue("@prVillageOrRoad", (nidDataResponse.success.data.presentAddress.villageOrRoad == null ? "" : nidDataResponse.success.data.presentAddress.villageOrRoad));
                insCmd.Parameters.AddWithValue("@prUpazila", (nidDataResponse.success.data.presentAddress.upozila == null ? "" : nidDataResponse.success.data.presentAddress.upozila));
                insCmd.Parameters.AddWithValue("@prUnionOrWard", (nidDataResponse.success.data.presentAddress.unionOrWard == null ? "" : nidDataResponse.success.data.presentAddress.unionOrWard));
                insCmd.Parameters.AddWithValue("@prPostOffice", (nidDataResponse.success.data.presentAddress.postOffice == null ? "" : nidDataResponse.success.data.presentAddress.postOffice));
                insCmd.Parameters.AddWithValue("@prPostalCode", (nidDataResponse.success.data.presentAddress.postalCode == null ? "" : nidDataResponse.success.data.presentAddress.postalCode));
                insCmd.Parameters.AddWithValue("@prWardForUnionPorishod", (nidDataResponse.success.data.presentAddress.wardForUnionPorishod == null ? "" : nidDataResponse.success.data.presentAddress.wardForUnionPorishod));
                insCmd.Parameters.AddWithValue("@prAdditionalMouzaOrMoholla", (nidDataResponse.success.data.presentAddress.additionalMouzaOrMoholla == null ? "" : nidDataResponse.success.data.presentAddress.additionalMouzaOrMoholla));
                insCmd.Parameters.AddWithValue("@prAdditionalVillageOrRoad", (nidDataResponse.success.data.presentAddress.additionalVillageOrRoad == null ? "" : nidDataResponse.success.data.presentAddress.additionalVillageOrRoad));
                insCmd.Parameters.AddWithValue("@prHomeOrHoldingNo", (nidDataResponse.success.data.presentAddress.homeOrHoldingNo == null ? "" : nidDataResponse.success.data.presentAddress.homeOrHoldingNo));
                insCmd.Parameters.AddWithValue("@prRegion", (nidDataResponse.success.data.presentAddress.region == null ? "" : nidDataResponse.success.data.presentAddress.region));
                insCmd.Parameters.AddWithValue("@formNumber", (nidDataResponse.success.data.formNo == null ? "" : nidDataResponse.success.data.formNo));
                insCmd.Parameters.AddWithValue("@voterArea", (nidDataResponse.success.data.voterArea == null ? "" : nidDataResponse.success.data.voterArea));
                insCmd.Parameters.AddWithValue("@voterAreaCode", (nidDataResponse.success.data.voterAreaCode == null ? "" : nidDataResponse.success.data.voterAreaCode));
                insCmd.Parameters.AddWithValue("@voterNumber", (nidDataResponse.success.data.voterNo == null ? "" : nidDataResponse.success.data.voterNo));
                insCmd.Parameters.AddWithValue("@birthPlace", (nidDataResponse.success.data.birthPlace == null ? "" : nidDataResponse.success.data.birthPlace));
                insCmd.Parameters.AddWithValue("@education", (nidDataResponse.success.data.education == null ? "" : nidDataResponse.success.data.education));
                insCmd.Parameters.AddWithValue("@photo", data);


                //SqlCommand rdCmd = new SqlCommand(rdQuery, conn);


                insCmd.CommandType = CommandType.Text;

                conn.Open();
                insCmd.ExecuteNonQuery();

                /*
                using (SqlDataReader rdr = rdCmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        persons.Add(new PersonModel()
                        {
                            Id = Convert.ToInt32(rdr["Id"]),
                            Name = Convert.ToString(rdr["Name"])
                        });
                    }
                }
                Message = "On Post";
                */
                conn.Close();
                fileInfo.Delete();

            }
        }

        /*************Obtain file path*************/
        private static string GetPhotoUrl()
        {
            string filename;
            string path = @"D:\Images";
            string[] files = Directory.GetFiles(path);

            if (files.Length == 1)
            {
                foreach (string file in files)
                {
                    filename = file;
                    return filename;
                }
            }
            else
            {
                return "";
                throw new Exception("More than one image");
            }
            return "";
        }
    }
}