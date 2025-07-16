<?php
require_once 'DbConnect.php';

header('Content-Type: application/json; charset=utf-8');

$username = $_POST['username'] ?? '';

if (empty($username)) {
    echo json_encode(['success' => false, 'message' => 'Error: Brak nazwy użytkownika']);
    exit;
}

$stmt = $pdo->prepare("SELECT level, gold, silver, exp FROM users WHERE username = ?");
$stmt->execute([$username]);
$userData = $stmt->fetch(PDO::FETCH_ASSOC);

if ($userData) {
    // Pobierz wymagane XP dla następnego levela
    $level = $userData['level'];
    $xpQuery = $pdo->prepare("SELECT xp_required FROM leveling WHERE level = ?");
    $xpQuery->execute([$level]);
    $levelData = $xpQuery->fetch(PDO::FETCH_ASSOC);

    $xpRequired = $levelData ? $levelData['xp_required'] : 0;

    // Dodaj xp_required do odpowiedzi
    $userData['xp_required'] = $xpRequired;

    echo json_encode(['success' => true, 'data' => $userData]);
} else {
    echo json_encode(['success' => false, 'message' => 'Error: Użytkownik nie znaleziony']);
}
