using NWRWS.Model.CouchDB;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Common
{
    public class CoutchDBFileManagement
    {

        private IHttpClientFactory httpClientFactory { get; set; }

        public CoutchDBFileManagement(IHttpClientFactory _httpClientFactory)
        {
            httpClientFactory = _httpClientFactory;
        }

        #region CouchDB Connector

        private HttpClient DbHttpClient()
        {
            var httpClient = this.httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.Timeout = new TimeSpan(0, 1440, 0);
            httpClient.BaseAddress = new Uri(ConfigDetailsValue.CouchDBURL);
            var dbUserByteArray = Encoding.ASCII.GetBytes(ConfigDetailsValue.CouchDBUser);
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(dbUserByteArray));
            return httpClient;
        }

        #endregion

        #region Attachment Selector

        internal class AttachmentExists
        {
            [JsonProperty("$exists")]
            public bool Exists { get; set; }
        }

        internal class FileTypeIn
        {
            [JsonProperty("$in")]
            public List<String> TypesIn { get; set; }
        }

        internal dynamic AttachmentsSelector()
        {
            FileTypeIn typeList = new FileTypeIn()
            {
                TypesIn = new List<string>() { "PDF", "PNG", "JPEG", "JPG", "GIF", "DOC", "DOCX", "XLS", "XLSX", "PPT", "PPTX" }
            };
            AttachmentExists isExists = new AttachmentExists()
            {
                Exists = true
            };

            var selector = new
            {
                selector = new
                {
                    FileExtension = typeList,
                    _attachments = isExists
                },
                fields = new ArrayList
                {
                     "_id",
                    "_rev",
                    "FileName",
                }
            };
            return selector;
        }

        #endregion

        #region CouchDB Functions

        public async Task<CoutchDBFileManagementResponse> AddAttachment(SaveCouchDBAttachment attachInfo)
        {
            try
            {
                var dbClient = DbHttpClient();
                var data = JsonContent.Create(attachInfo);
                var postResult = await dbClient.PostAsync(ConfigDetailsValue.CouchDBDbName, data).ConfigureAwait(true);

                var result = await postResult.Content.ReadAsStringAsync();
                
                var savedInfo = JsonConvert.DeserializeObject<SavedCouchDBResult>(result,
                                new JsonSerializerSettings
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                });
                var requestContent = new ByteArrayContent(attachInfo.AttachmentData);

                requestContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                var putResult = await dbClient.PutAsync(ConfigDetailsValue.CouchDBDbName + "/" +
                                                        savedInfo.Id + "/" +
                                                        attachInfo.FileName + "?rev=" + savedInfo.Rev, requestContent).ConfigureAwait(true);

                if (putResult.IsSuccessStatusCode)
                {
                    return new CoutchDBFileManagementResponse { IsSuccess = true, Result = await putResult.Content.ReadAsStringAsync() };
                }
                return new CoutchDBFileManagementResponse { IsSuccess = false, Result = putResult.ReasonPhrase };
            }
            catch (WebException exception)
            {
                ErrorLogger.Error(exception.Message.ToString(), exception.ToString());
                string responseText;

                using (var reader = new StreamReader(exception.Response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }
                return new CoutchDBFileManagementResponse { IsSuccess = false, Result = responseText };
            }
            catch (Exception exception)
            {
                ErrorLogger.Error(exception.Message.ToString(), exception.ToString());
                return new CoutchDBFileManagementResponse { IsSuccess = false, Result = exception.ToString() };
            }
        }

        public async Task<CoutchDBFileManagementResponse> UpdateAttachment(SaveCouchDBAttachment attachInfo)
        {
            var dbClient = DbHttpClient();

            var data = JsonContent.Create(attachInfo);
            var postResult = await dbClient.PutAsync(ConfigDetailsValue.CouchDBDbName + "/" + attachInfo.Id + "?rev=" + attachInfo.Rev, data);

            var result = await postResult.Content.ReadAsStringAsync();

            var savedInfo = JsonConvert.DeserializeObject<SavedCouchDBResult>(result,
                            new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            });
            attachInfo.Id = savedInfo.Id;
            attachInfo.Rev = savedInfo.Rev;

            var requestContent = new ByteArrayContent(attachInfo.AttachmentData);
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

            var putResult = await dbClient.PutAsync(ConfigDetailsValue.CouchDBDbName + "/" +
                                                    savedInfo.Id + "/" +
                                                    attachInfo.FileName + "?rev=" + savedInfo.Rev, requestContent);

            if (putResult.IsSuccessStatusCode)
            {
                return new CoutchDBFileManagementResponse { IsSuccess = true, Result = await putResult.Content.ReadAsStringAsync() };
            }
            return new CoutchDBFileManagementResponse { IsSuccess = false, Result = putResult.ReasonPhrase };
        }

        public async Task<dynamic> FindAttachments()
        {
            var dbClient = DbHttpClient();
            var docSelector = AttachmentsSelector();

            var data = JsonContent.Create(docSelector);

            //var jsonQuery = JsonConvert.SerializeObject(docSelector, Formatting.Indented,
            //                new JsonSerializerSettings
            //                {
            //                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //                });
            var dbResult = await dbClient.PostAsync(ConfigDetailsValue.CouchDBDbName + "/_find", data);

            if (dbResult.IsSuccessStatusCode)
            {
                return new CoutchDBFileManagementResponse { IsSuccess = true, Result = await dbResult.Content.ReadAsStringAsync() };
            }
            return new CoutchDBFileManagementResponse  { IsSuccess = false, Result = dbResult.ReasonPhrase };
        }

        public async Task<dynamic> GetDocumentAsync(string id)
        {
            var dbClient = DbHttpClient();
            var dbResult = await dbClient.GetAsync(ConfigDetailsValue.CouchDBDbName + "/" + id);

            if (dbResult.IsSuccessStatusCode)
            {
                return new CoutchDBFileManagementResponse { IsSuccess = true, Result = await dbResult.Content.ReadAsStringAsync() };
            }
            return new CoutchDBFileManagementResponse { IsSuccess = false, Result = dbResult.ReasonPhrase };
        }

        public async Task<CoutchDBFileManagementResponse> DeleteDocumentAsync(string id, string rev)
        {
            var dbClient = DbHttpClient();
            var dbResult = await dbClient.DeleteAsync(ConfigDetailsValue.CouchDBDbName + "/" + id + "?rev=" + rev);

            if (dbResult.IsSuccessStatusCode)
            {
                return new CoutchDBFileManagementResponse { IsSuccess = true, Result = await dbResult.Content.ReadAsStringAsync() };
            }
            return new CoutchDBFileManagementResponse { IsSuccess = false, Result = dbResult.ReasonPhrase };
        }

        public async Task<dynamic> GetAttachmentByteArray(string DocId, string AttName)
        {
            var dbClient = DbHttpClient();
            var dbResult = await dbClient.GetAsync(ConfigDetailsValue.CouchDBDbName + "/" + DocId + "/" + AttName);

            if (dbResult.IsSuccessStatusCode)
            {
                return new CoutchDBFileManagementResponse { IsSuccess = true, Result = await dbResult.Content.ReadAsByteArrayAsync() };
            }
            return new CoutchDBFileManagementResponse { IsSuccess = false, Result = dbResult.ReasonPhrase };
        }

        #endregion
    }
}
