using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DCF.Common.Exceptions;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.ConnectedServices.FileUpload;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.Model.Interface.FileUpload;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class FileUploadDomain : IFileUploadDomain
    {
        #region Properties

        private readonly IUnitOfWork         _unitOfWork;
        private readonly IFileUploadConfig   _fileUploadConfig;
        private readonly IDocumentRepository _docRepository;

        #endregion

        #region Methods

        public FileUploadDomain(IDocumentRepository docRepository, IUnitOfWork unitOfWork, IFileUploadConfig fileUploadConfig)
        {
            _unitOfWork       = unitOfWork;
            _fileUploadConfig = fileUploadConfig;
            _docRepository    = docRepository;
        }


        /// <summary>
        /// Uploads a file to CM -- Not yet used but if we have FIle upload control in UI this API will be used
        /// save process to complete
        /// </summary>
        /// <param name="file"></param>
        /// <param name="pin"></param>
        /// <param name="employabilityPlanId"></param>
        public bool UploadFile(MemoryStream file, string pin, int employabilityPlanId)
        {
            if (file == null)
                return false;

            if (file.Length <= 0) return false;

            var docId = SaveDocument(employabilityPlanId);

            if (docId <= 0) return false;

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                var fileName  = $"EmployabilityPlan_{pin}_{employabilityPlanId.ToString()}.pdf";

                ICMServiceRequest cmServiceReq = new CMServiceRequest()
                                                 {
                                                     CMServiceURL = _fileUploadConfig.Endpoint,
                                                     CMServerName = _fileUploadConfig.CMServerName,
                                                     CMUserID     = _fileUploadConfig.CMUserId,
                                                     CMUserPwd    = _fileUploadConfig.CMUserPwd
                                                 };

                FileUpload.GetAuthentication(ref cmServiceReq);
                FileUpload.Store(ref cmServiceReq, fileBytes, "", "", "",
                                 "", "", fileName, docId, hfsPin: pin);
            }

            return true;
        }

        /// <summary>
        /// Called to save into WWP DB
        /// </summary>
        /// <returns></returns>
        private int SaveDocument(int employabilityPlanId)
        {
            var modifiedDate = DateTime.Now;
            var docList      = _docRepository.New();

            docList.EmployabilityPlanId = employabilityPlanId;
            docList.UploadedDate        = modifiedDate;
            docList.IsDeleted           = false;
            docList.IsScanned           = false;
            docList.ModifiedBy          = "WWP";
            docList.ModifiedDate        = DateTime.Now;

            _docRepository.Add(docList);
            _unitOfWork.Commit();

            var docId = docList.Id;

            return docId;
        }


        /// <summary>
        /// Retrieves the document from ECF
        /// </summary>
        /// <exception cref="System.Exception"></exception>
        public byte[] RetrieveDocument(string docId)
        {
            // Create a request to Content Manager

            ICMServiceRequest cmServiceReq = new CMServiceRequest()
                                             {
                                                 CMServiceURL = _fileUploadConfig.Endpoint,
                                                 CMServerName = _fileUploadConfig.CMServerName,
                                                 CMUserID     = _fileUploadConfig.CMUserId,
                                                 CMUserPwd    = _fileUploadConfig.CMUserPwd
                                             };

            // Authenticate document request
            FileUpload.GetAuthentication(ref cmServiceReq);

            // Get document from process layer
            var searchResult = FileUpload.Retrieve(ref cmServiceReq, docId);

            return searchResult;
        }

        /// <summary>
        /// Retrieves All documents from CM for a pin
        /// </summary>
        /// <param name="pinNumber"></param>
        /// <returns></returns>
        public List<DocumentRequest> RetrieveAllDocument(string pinNumber)
        {
            // Create a request to Content Manager

            var docs = new List<DocumentRequest>();

            ICMServiceRequest cmServiceReq = new CMServiceRequest
                                             {
                                                 CMServiceURL = _fileUploadConfig.Endpoint,
                                                 CMServerName = _fileUploadConfig.CMServerName,
                                                 CMUserID     = _fileUploadConfig.CMUserId,
                                                 CMUserPwd    = _fileUploadConfig.CMUserPwd
                                             };

            // Authenticate document request
            FileUpload.GetAuthentication(ref cmServiceReq);

            // Get document from process layer
            var searchResult = FileUpload.RetrieveAllDocuments(ref cmServiceReq, pinNumber);
            searchResult?.ResultSet?.Item.ForEach(i =>
                                                  {
                                                      var doc = new DocumentRequest
                                                                {
                                                                    DocumentId   = i.ItemXML.ECF_Document.HFS_CorrCntlNum,
                                                                    UploadedDate = Convert.ToDateTime(i.ItemXML.ECF_Document.ECF_ReceiveDate),
                                                                    IsDeleted    = false,
                                                                    IsScanned    = true,
                                                                    ModifiedBy   = "ECF",
                                                                    ModifiedDate = Convert.ToDateTime(i.ItemXML.ECF_Document.ECF_ReceiveDate)
                                                                };
                                                      docs.Add(doc);
                                                  });


            return docs;
        }

        public bool UploadEPDoc(string pin, int employabilityPlanId, byte[] contractFileStream)
        {
            using (var tx = _docRepository.GetDataBase().BeginTransaction())
            {
                try
                {
                    var docId = SaveDocument(employabilityPlanId);

                    if (docId <= 0) return false;
                    ICMServiceRequest cmServiceReq = new CMServiceRequest
                                                     {
                                                         CMServiceURL = _fileUploadConfig.Endpoint,
                                                         CMServerName = _fileUploadConfig.CMServerName,
                                                         CMUserID     = _fileUploadConfig.CMUserId,
                                                         CMUserPwd    = _fileUploadConfig.CMUserPwd
                                                     };
                    FileUpload.GetAuthentication(ref cmServiceReq);

                    FileUpload.Store(ref cmServiceReq, contractFileStream, "", "", "",
                                     "", "", pin + $"EmployabilityPlan_{pin}_{employabilityPlanId.ToString()}.pdf", docId, hfsPin: pin);

                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Dispose();
                    throw new DCFApplicationException("Failed due to File Upload issue. Please try again.", ex);
                }

                tx.Dispose();
            }
            return true;
        }

        #endregion
    }
}
