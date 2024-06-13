<?php
/**
 * Plugin Name: Chipin
 * Description: A WordPress plugin with custom endpoints for chipin integration
 * Version: 1.0.0
 */

 /**
  * Register a custom endpoint to receive data from Chipin
  */
add_action('rest_api_init', function () {
    register_rest_route('Chipin/v1', '/receive-data', array(
        'methods' => 'POST',
        'callback' => 'process_received_data',
    ));
});

add_action('rest_api_init', function () {
    register_rest_route('Chipin/v1', '/initiate', array(
        'methods' => 'POST',
        'callback' => 'send_session',
    ));
});

/**
 * Register a custom endpoint to send data to Chipin
 */
add_action('rest_api_init', function () {
    register_rest_route('Chipin/v1', '/receive-id', array(
        'methods' => 'POST',
        'callback' => 'send_cart',
    ));
});

add_action('wp_enqueue_scripts', 'my_enqueue_scripts');

function send_session(WP_REST_Request $request) {
    // get session_id from request
    $data = $request->get_json_params();
    $session_id = $data['sessionId'];
    $return_url = "https://test.bluebeyond.co.za/wp-json/Chipin/v1/receive-id";

    $send_data = array(
        'SessionId' => $session_id,
        'ReturnUrl' => $return_url
    );

    //post to chipin
    $url = 'https://c136-160-19-235-118.ngrok-free.app/api/TempConnection';
    
    $response = wp_remote_post($url, $send_data);
    return $response;
}

    // Enqueue scripts and localize them with the session ID
function my_enqueue_scripts() {
    // Enqueue your script
    wp_enqueue_script('8368', 'https://test.bluebeyond.co.za/wp-content/uploads/custom-css-js/8368.js', array('jquery'), '1.0', true);

    // Check if WooCommerce is active and a session exists
    if (class_exists('WooCommerce') && WC()->session) {
        // Get the session ID
        $session_id = WC()->session->get_customer_id();

        // Localize the script with the session ID
        wp_localize_script('8368', 'objSesh', array(
            'session_id' => $session_id
        ));
    }
}

function send_cart(WP_REST_Request $request) {
   
    // Sample JSON data to be sent as response
    try {


// Query the wp_woocommerce_sessions table to get all session IDs


global $wpdb;
		
	$data = $request->get_json_params();
    $chipin_id = $data['ChipinId'];

$session_key = '8845';

// Query the wp_woocommerce_sessions table to get the session data for the specific session key
$session_id = $data['Cart'];
		
$session_data = $wpdb->get_var($wpdb->prepare("SELECT session_value FROM {$wpdb->prefix}woocommerce_sessions WHERE session_key = %s", $session_id));

// The session data is stored as a serialized string, so you need to unserialize it
$session_data = maybe_unserialize($session_data);

// Check if the session data contains cart data
$cart_data = '';
if (isset($session_data['cart'])) {
    // The session data contains cart data, you can print it for debugging
    $cart_data = unserialize($session_data['cart']);
} else {
    // The session data does not contain cart data
    return 'dddddd';
}
        
	
   

$products = [];
foreach ($cart_data as $cart_item_key => $cart_item) {
    // Get the product
    $product = wc_get_product($cart_item['product_id']);

    if ($product) {
        // The product exists, add it to the products array
        $products[] = [
            'ProductName' => $product->get_name(),
            'ProductDescription' => $product->get_description(),
            'Price' => $product->get_price(),
            'Image' => wp_get_attachment_url($product->get_image_id()),
            'Store' => 'Brand Deals', // This needs to be adjusted based on where you get the store info
            'CustString1' => 'CustString1', // This needs to be adjusted to any string that you want returned to you
            'CustInt1' => 99, // This needs to be adjusted to any integer that you want returned to you
            'ProductId' => $product->get_id(),
            'ReturnUrl' => 'https://test.bluebeyond.co.za/wp-json/Chipin/v1/receive-data', // This needs to be adjusted based on where you get the ReturnUrl info
            'Quantity' => $cart_item['quantity']
        ];
    } else {
        // The product does not exist
        error_log("The product does not exist");
    }
}
	





    $response_data = array(
        'ChipinId' => $chipin_id,
        "Products" => $products, 
    );
	
    $dataString = stringer($response_data);

    $test = md5($dataString);
    // Set response headers
   // $response_data->set_status(200);
    $response_data['Signature'] = $test;

    return $response_data;

    } catch (Exception $e) {
        return $e;
    }
    
}


function stringer($response_data) {
	$dataString = '';
	foreach ($response_data as $key => $value) {
    if ($key === 'Products') {
        foreach ($value as $index => $product) {
            foreach ($product as $productKey => $productValue) {
                $dataString .= "{$key}[{$index}].{$productKey}={$productValue}\n";
            }
        }
    } else {
        $dataString .= "{$key}={$value}\n";
    }
}
	return $dataString;
}

function generateSignature($data, $secretKey) {
    $jsonData = json_encode($data);
    $hash = hash_hmac('sha256', $jsonData, $secretKey, true);
    return bin2hex($hash);
}

function process_received_data(WP_REST_Request $request) {
    $data = $request->get_json_params();
    
    // Extract necessary data from the JSON
    $shipping_address = format_shipping_data_for_woocommerce($data);
    $billing_address = format_billing_data_for_woocommerce($data);
    $products = $data['Products'];

    // Create an order
    $order = wc_create_order();

    // Add shipping address
    $order->set_address($shipping_address, 'shipping');

    // Add billing address
    $order->set_address($billing_address, 'billing');

    // Add products to the order
    foreach ($products as $product_data) {
        $product_id = $product_data['ProductId'];
        $quantity = $product_data['Quantity']; 
        
        $order->add_product(get_product_2($product_id), $quantity);
    }

    // Calculate totals and save order
    $order->calculate_totals();
    $order_id = $order->save();

    // Return the order ID or any other relevant response
    return array('order_id' => $order_id);
}

// Helper function to get a product by ID
function get_product_2($product_id) {
    return wc_get_product($product_id);
}

function format_shipping_data_for_woocommerce($data) {
    $formatted_data = array( 
        'first_name' => $data['ShippingAddress']['FirstName'],
        'last_name' => $data['ShippingAddress']['LastName'],
        'address_1' => $data['ShippingAddress']['Address1'],
        'address_2' => $data['ShippingAddress']['Address2'],
        'city' => $data['ShippingAddress']['City'],
        'state' => $data['ShippingAddress']['State'],
        'postcode' => $data['ShippingAddress']['PostCode'],
        'country' => $data['ShippingAddress']['Country'],
        'email' => $data['ShippingAddress']['Email'],
        'phone' => $data['ShippingAddress']['PhoneNumber'],
    );

    return $formatted_data;
}

function format_billing_data_for_woocommerce($data) {
    $formatted_data = array(
        'first_name' => $data['BillingAddress']['FirstName'],
        'last_name' => $data['BillingAddress']['LastName'],
        'address_1' => $data['BillingAddress']['Address1'],
        'address_2' => $data['BillingAddress']['Address2'],
        'city' => $data['BillingAddress']['City'],
        'state' => $data['BillingAddress']['State'],
        'postcode' => $data['BillingAddress']['PostCode'],
        'country' => $data['BillingAddress']['Country'],
        'email' => $data['BillingAddress']['Email'],
        'phone' => $data['BillingAddress']['PhoneNumber'],
    );

    return $formatted_data;
}