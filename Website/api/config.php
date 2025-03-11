<?php
function ReturnMessage($code = 500, $message = "") {   
    $return = array(
        "code" => $code,
        "message" => $message
    );
    ReturnResult($return, $code);
}

function ReturnResult($result_array, $code = 200) {
    header("Content-Type: application/json");    
    echo json_encode($result_array);
    http_response_code($code);
    die();
}

function ReturnQuery($sql) {
    global $db;
    require_once "connection.php";

    $result = $db->query($sql);
    
    if (!$result) {    
        ReturnMessage(500, "Hiba az SQL lekérdezésben: (Hibakód: $db->errno) $db->error");
    }
    
    ReturnResult($result->fetch_all(MYSQLI_ASSOC));
}

function checkValidity($method, ...$fields) {
    if ($_SERVER['REQUEST_METHOD'] != $method) {
        ReturnMessage(405, "Hiba az API-hívásban.");
    }
    if (!hasProperFields($fields)) {
        ReturnMessage(400, "Hiba az API-hívásban.");
    }
}

function hasProperFields($fields) {
    if (count($_REQUEST) != count($fields)) {return false;}
    foreach ($fields as $f) { if (!isset($_REQUEST[$f])) { return false; } }
    return true;
}