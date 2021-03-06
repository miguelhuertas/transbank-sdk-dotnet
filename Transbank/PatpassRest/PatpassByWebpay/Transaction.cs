﻿using System;
using Newtonsoft.Json;
using Transbank.Common;
using Transbank.Exceptions;
using Transbank.Patpass.Common;
using Transbank.Patpass.PatpassByWebpay.Requests;
using Transbank.Patpass.PatpassByWebpay.Responses;
using RequestService = Transbank.Common.RequestService;

namespace Transbank.Patpass.PatpassByWebpay
{
    public static class Transaction
    {
        private static string _commerceCode = "597055555550";
        private static string _apiKey = "579B532A7440BB0C9079DED94D31EA1615BACEB56610332264630D42D0A36B1C";
        private static PatpassByWebpayIntegrationType _integrationType = PatpassByWebpayIntegrationType.Test;
        
        private static string _commerceCodeHeaderName = "Tbk-Api-Key-Id";
        private static string _apiKeyHeaderName = "Tbk-Api-Key-Secret";

        private static RequestServiceHeaders _headers = new RequestServiceHeaders(_apiKeyHeaderName, _commerceCodeHeaderName);

        public static RequestServiceHeaders Headers
        {
            get => _headers;
            set => _headers = value ?? throw new ArgumentNullException(
                                  nameof(value), "Integration type can't be null."
                              );
        }
        public static string CommerceCode
        {
            get => _commerceCode;
            set => _commerceCode = value ?? throw new ArgumentNullException(
                nameof(value), "Commerce code can't be null."
            );
        }

        public static string ApiKey
        {
            get => _apiKey;
            set => _apiKey = value ?? throw new ArgumentNullException(
                nameof(value), "Api Key can't be null."
            );
        }

        public static PatpassByWebpayIntegrationType IntegrationType
        {
            get => _integrationType;
            set => _integrationType = value ?? throw new ArgumentNullException(
                nameof(value), "Integration type can't be null."
            );
        }

        

        public static Options DefaultOptions()
        {
            return new Options(CommerceCode, ApiKey, IntegrationType, Headers);
        }

        public static CreateResponse Create(string buyOrder, string sessionId, decimal amount, string returnUrl, string serviceId, string cardHolderId,
                string cardHolderName, string cardHolderLastName1, string cardHolderLastName2, string cardHolderMail, string cellphoneNumber,
                string expirationDate, string commerceMail, bool ufFlag)
        {
            return Create(buyOrder, sessionId, amount, returnUrl, serviceId, cardHolderId,
                cardHolderName, cardHolderLastName1, cardHolderLastName2, cardHolderMail, cellphoneNumber,
                expirationDate, commerceMail, ufFlag, DefaultOptions());
        }

        public static CreateResponse Create(string buyOrder, string sessionId, decimal amount, string returnUrl, string serviceId, string cardHolderId,
                string cardHolderName, string cardHolderLastName1, string cardHolderLastName2, string cardHolderMail, string cellphoneNumber,
                string expirationDate, string commerceMail, bool ufFlag, Options options)
        {
            var createRequest = new CreateRequest(buyOrder, sessionId, amount, returnUrl, serviceId, cardHolderId,
                cardHolderName, cardHolderLastName1, cardHolderLastName2, cardHolderMail, cellphoneNumber,
                expirationDate, commerceMail, ufFlag);
            var response = RequestService.Perform<TransactionCreateException>(createRequest, options);

            return JsonConvert.DeserializeObject<CreateResponse>(response);
        }

        public static CommitResponse Commit(string token)
        {
            return Commit(token, DefaultOptions());
        }

        public static CommitResponse Commit(string token, Options options)
        {
            return ExceptionHandler.Perform<CommitResponse, TransactionCommitException>(() =>
            {
                var commitRequest = new CommitRequest(token);
                var response = RequestService.Perform<TransactionCommitException>(
                    commitRequest, options);

                return JsonConvert.DeserializeObject<CommitResponse>(response);
            });
        }

        public static StatusResponse Status(string token)
        {
            return Status(token, DefaultOptions());
        }

        public static StatusResponse Status(string token, Options options)
        {
            return ExceptionHandler.Perform<StatusResponse, TransactionStatusException>(() =>
            {
                var statusRequest = new StatusRequest(token);
                var response = RequestService.Perform<TransactionStatusException>(
                    statusRequest, options);

                return JsonConvert.DeserializeObject<StatusResponse>(response);
            });
        }
    }
}
