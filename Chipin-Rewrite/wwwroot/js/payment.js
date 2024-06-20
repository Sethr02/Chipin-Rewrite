

async function getPaymentKey() {
   
    const cartTotalRaw = sessionStorage.getItem('cartTotalRaw');
    const orderId = sessionStorage.getItem('orderId');

    const data = {
        Amount: parseFloat(cartTotalRaw),
        MerchantReference: 'Chipin',
        Token: '',
        OrderId: parseInt(orderId)
    };

    try {
        const response = await fetch('/CallPay/GetKey', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data),
            credentials: 'include' // Add this line if you need to include cookies or other credentials
        });

        if (response.ok) {
            const result = await response.json();
            window.location.href = result.url;
        } else {
            console.error('Failed to get payment key:', response.status);
        }
    } catch (error) {
        console.error('Error:', error);
    }
}