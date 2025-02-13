<?php
function ReturnError($code = 500, $message = "") {
    header("Content-Type: application/json");    
    $return = array(
        "code" => $code,
        "message" => $message
    );
    echo json_encode($return, JSON_PRETTY_PRINT);
    http_response_code($code);
    die();
}


function ReturnResult($result_array, $code = 200) {
    header("Content-Type: application/json");    
    echo json_encode($result_array);
    http_response_code($code);
    die();
}

function ReturnQuery($sql) {
    require "connection.php";
    $result = $db->query($sql);
    
    if (!$result) {    
        //ReturnError(500, "Hiba az SQL lekérdezésben: (Hibakód: $db->errno) $db->error");
        ReturnError(500, "Baj az SQL-lel");
    }
    
    ReturnResult($result->fetch_all(MYSQLI_ASSOC));
}

function checkValidity($method, ...$fields) {
    if ($_SERVER['REQUEST_METHOD'] != $method) {
        ReturnError(405, "Hiba az API-hívásban.");
    }
    if (!hasProperFields($fields)) {
        ReturnError(400, "Hiba az API-hívásban.");
    }
}

function hasProperFields($fields) {
    if (count($_REQUEST) != count($fields)) {return false;}
    foreach ($fields as $f) { if (!isset($_REQUEST[$f])) { return false; } }
    return true;
}