<?php
header("Content-Type: application/json");

function ReturnError($code = 500, $message = "") {
    $return = array(
        "code" => $code,
        "message" => $message
    );
    echo json_encode($return, JSON_PRETTY_PRINT);
    http_response_code($code);
    die();
}

function ReturnQuery($sql) {
    require_once "connection.php";
    $result = $db->query($sql);
    
    if (!$result) {    
        ReturnError(500, "Hiba az SQL lekérdezésben: (Hibakód: $db->errno) $db->error");
    }
    
    $return = array();
    while ($line = $result->fetch_assoc()) {
        $return[] = $line;
    }
    ReturnResult($return);
}

function ReturnResult($result_array, $code = 200) {
    echo json_encode($result_array, JSON_PRETTY_PRINT);
    http_response_code($code);
}

function checkProperFields($method, ...$fields) {
    if (!hasProperFields($method, $fields)) {
        ReturnError(400, "Hiba az API-hívásban.");
    }
}

function hasProperFields($method, $fields) {
    if (count($_REQUEST) != count($fields)) {return false;}
    foreach ($fields as $f) { if (!isset($_REQUEST[$f])) { return false; } }
    return true;
}