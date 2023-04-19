namespace BlazorApplePay {
    class BlazorApplePay {
        public async addOnClick(element: HTMLElement, dotNetHelper) {
            element.addEventListener(
                'click', 
                async () => await dotNetHelper.invokeMethodAsync('OnClickAsync'))
        }

        public createSession(sessionSerialized, dotNetHelper) {
            const session = JSON.parse(sessionSerialized);
            const applePaySession = new ApplePaySession(session.version, session.request);

            applePaySession.onvalidatemerchant = async event => {
                const merchantSession = await dotNetHelper.invokeMethodAsync('OnValidateMerchantAsync', event.validationURL);
                applePaySession.completeMerchantValidation(merchantSession);
            };

            applePaySession.onpaymentauthorized = async event => {
                const token = btoa(JSON.stringify(event.payment.token.paymentData))
                await dotNetHelper.invokeMethodAsync('OnPaymentAuthorization', token);
                
                const result = {
                    "status": ApplePaySession.STATUS_SUCCESS
                };
                applePaySession.completePayment(result);
            };
        
            applePaySession.oncancel = event => {
                // Payment cancelled by WebKit
            };
        
            applePaySession.begin();
        }
    }

    export function Load(): void {
        window['blazorApplePay'] = new BlazorApplePay();
    }
}

BlazorApplePay.Load();