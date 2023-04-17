blazorApplePay = {
    addOnClick: async (id, dotNetHelper) => {
        const button = document.getElementById(id);
        if (!button) return;

        button.addEventListener('click', async () => await dotNetHelper.invokeMethodAsync('OnClickAsync'))
    },
    createSession: (session, dotNetHelper) => {
        const applePaySession = new ApplePaySession(session.version, session.request);

        applePaySession.onvalidatemerchant = async event => {
            // Call your own server to request a new merchant session.
            const merchantSession = await dotNetHelper.invokeMethodAsync('OnValidateMerchantAsync', event.validationURL);
            console.log(merchantSession);
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