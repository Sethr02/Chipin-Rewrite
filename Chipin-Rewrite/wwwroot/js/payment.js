﻿async function getPaymentKey() {
    const orderId = '@ViewBag.OrderId';

    try {
        const sessionData = await response.json();
        const data = {
            Amount: sessionData.cartTotal,
            MerchantReference: 'Chipin',
            Token: '',
        };

        const paymentResponse = await fetch('/CallPay/GetKey', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data),
            credentials: 'include'
        });

        if (paymentResponse.ok) {
            const result = await paymentResponse.json();
            window.location.href = result.url;
        } else {
            console.error('Failed to get payment key:', paymentResponse.status);
        }
    } catch (error) {
        console.error('Error:', error);
    }
}