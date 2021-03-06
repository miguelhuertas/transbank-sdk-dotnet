﻿using Transbank.PatPass;

namespace Transbank.Webpay
{
    public class Webpay
    {
        WebpayNormal _normalTransaction;
        WebpayOneClick _oneclickTransaction;
        WebpayNullify _nullifyTransaction;
        WebpayMallNormal _mallNormalTransaction;
        WebpayCapture _captureTransaction;
        WebpayComplete _completeTransaction;
        PatPassByWebpayNormal _patpassbywebpaynormalTransaction;        
        readonly Configuration configuration;

        private static readonly object padlock = new object();

        public Webpay(Configuration param)
        {
            if (string.IsNullOrEmpty(param.WebpayCertPath))
                switch (param.Environment)
                {
                    case "PRODUCCION":
                        param.WebpayCertPath = Configuration.GetProductionPublicCertPath();
                        break;
                    default:
                        param.WebpayCertPath = Configuration.GetTestingPublicCertPath();
                        break;
                }                     
            configuration = param;
        }

        public WebpayNormal NormalTransaction
        {
            get
            {
                if (_normalTransaction == null)
                    lock(padlock)
                        if (_normalTransaction == null)
                            _normalTransaction = new WebpayNormal(configuration);
                return _normalTransaction;
            }
        }

        public WebpayOneClick OneClickTransaction
        {
            get
            {
                if (_oneclickTransaction == null)
                    lock(padlock)
                        if (_oneclickTransaction == null)
                            _oneclickTransaction = new WebpayOneClick(configuration);
                return _oneclickTransaction;
            }
        }

        public WebpayMallNormal MallNormalTransaction
        {
            get
            {
                if (_mallNormalTransaction == null)
                    lock(padlock)
                        if (_mallNormalTransaction == null)
                            _mallNormalTransaction = new WebpayMallNormal(configuration);
                return _mallNormalTransaction;
            }
        }

        public WebpayNullify NullifyTransaction
        {
            get
            {
                if (_nullifyTransaction == null)
                    lock(padlock)
                        if (_nullifyTransaction == null)
                            _nullifyTransaction = new WebpayNullify(configuration);
                return _nullifyTransaction;

            }
        }

        public WebpayCapture CaptureTransaction
        {
            get
            {
                if (_captureTransaction == null)
                    lock(padlock)
                        if (_captureTransaction == null)
                            _captureTransaction = new WebpayCapture(configuration);
                return _captureTransaction;
            }
        }

        public WebpayComplete CompleteTransaction
        {
            get
            {
                if (_completeTransaction == null)
                    lock(padlock)
                        if (_completeTransaction == null)
                            _completeTransaction = new WebpayComplete(configuration);
                return _completeTransaction;
            }
        }

        public PatPassByWebpayNormal PatPassByWebpayTransaction
        {
            get
            {
                if (_patpassbywebpaynormalTransaction == null)
                    lock (padlock)
                        if (_patpassbywebpaynormalTransaction == null)
                            _patpassbywebpaynormalTransaction = new PatPassByWebpayNormal(configuration);
                return _patpassbywebpaynormalTransaction;
            }
        }
    }
}
