<?php
  
require_once("config.php");

$method = $_SERVER['REQUEST_METHOD'];

$db = new mysqli($host, $user, $pw, $db);

if ($db->connect_errno > 0) {
  http_response_code(500);
  die("Error: Connect");
}

$response = "";
switch ($method) {
  case 'GET':
    $response = getResponse($db);
    break;
  case 'POST':
    $response = postResponse($db);
    break;
  default:
    $response = errorResponse();
    http_response_code(500);
    break;
}

echo $response;

function getResponse($db) {
  http_response_code(200);
  $sql = <<<SQL
  select name, score
  from highscore
  order by score desc
  limit 10
SQL;
  if (!$dbResult = $db->query($sql)) {
    http_response_code(500);
    die('Error: GET');
  }
  
  $result = '{"highscore":[';
  $first = 1;
  while ($row = $dbResult->fetch_assoc()) {
    if (!$first) {
      $result .= ',';
    }
    $result .= '{"name":"' . $row['name'] . '", "score": ' . $row['score'] . '}';
    $first = 0;
  }
  $dbResult->free();
  
  $result .= ']}';
  
  return $result;
}

function postResponse($db) {
  if (strlen($_POST['name']) < 3) {
    http_response_code(500);
    die("Error: Too Short");
  }
  
  $sql = <<<SQL
  insert into 
    highscore (name, score)
  values
    (?, ?)
SQL;
  $statement = $db->prepare($sql);
  $statement->bind_param('si', $_POST['name'], $_POST['score']);
  if (!$statement->execute()) {
    http_response_code(500);
    die("Error: POST");
  }
  $statement->close();
  
  
  http_response_code(200);
}

function errorResponse() {
  http_response_code(500);
  
}

?>