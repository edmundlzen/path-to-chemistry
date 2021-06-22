extends Node2D

var tt2 = {
	'1':'1','2':'2','3':'3',
	'4':'4','5':'5','6':'6',
	'7':'7','8':'8','9':'9'
}
var AI = ['1','2','3','4','5','6','7','8','9']
var player = '1'
var moves = 0

func check():
	if (tt2['1'] == 'X' and tt2['2'] == 'X' and tt2['3'] == 'X'):
		get_node("ColorRect/Title/Label").text = 'Player One Won!'
	elif (tt2['4'] == 'X' and tt2['5'] == 'X' and tt2['6'] == 'X'):
		get_node("ColorRect/Title/Label").text = 'Player One Won!'
	elif (tt2['7'] == 'X' and tt2['8'] == 'X' and tt2['9'] == 'X'):
		get_node("ColorRect/Title/Label").text = 'Player One Won!'
	elif (tt2['1'] == 'X' and tt2['4'] == 'X' and tt2['7'] == 'X'):
		get_node("ColorRect/Title/Label").text = 'Player One Won!'
	elif (tt2['2'] == 'X' and tt2['5'] == 'X' and tt2['8'] == 'X'):
		get_node("ColorRect/Title/Label").text = 'Player One Won!'
	elif (tt2['3'] == 'X' and tt2['6'] == 'X' and tt2['9'] == 'X'):
		get_node("ColorRect/Title/Label").text = 'Player One Won!'
	elif (tt2['1'] == 'X' and tt2['5'] == 'X' and tt2['9'] == 'X'):
		get_node("ColorRect/Title/Label").text = 'Player One Won!'
	elif (tt2['3'] == 'X' and tt2['5'] == 'X' and tt2['7'] == 'X'):
		get_node("ColorRect/Title/Label").text = 'Player One Won!'
	elif (tt2['1'] == 'O' and tt2['2'] == 'O' and tt2['3'] == 'O'):
		get_node("ColorRect/Title/Label").text = 'Player Two Won!!'
	elif (tt2['4'] == 'O' and tt2['5'] == 'O' and tt2['6'] == 'O'):
		get_node("ColorRect/Title/Label").text = 'Player Two Won!!'
	elif (tt2['7'] == 'O' and tt2['8'] == 'O' and tt2['9'] == 'O'):
		get_node("ColorRect/Title/Label").text = 'Player Two Won!!'
	elif (tt2['1'] == 'O' and tt2['4'] == 'O' and tt2['7'] == 'O'):
		get_node("ColorRect/Title/Label").text = 'Player Two Won!!'
	elif (tt2['2'] == 'O' and tt2['5'] == 'O' and tt2['8'] == 'O'):
		get_node("ColorRect/Title/Label").text = 'Player Two Won!!'
	elif (tt2['3'] == 'O' and tt2['6'] == 'O' and tt2['9'] == 'O'):
		get_node("ColorRect/Title/Label").text = 'Player Two Won!!'
	elif (tt2['1'] == 'O' and tt2['5'] == 'O' and tt2['9'] == 'O'):
		get_node("ColorRect/Title/Label").text = 'Player Two Won!!'
	elif (tt2['3'] == 'O' and tt2['5'] == 'O' and tt2['7'] == 'O'):
		get_node("ColorRect/Title/Label").text = 'Player Two Won!!'
	elif (moves == 9):
		get_node("ColorRect/Title/Label").text = 'Draw!!!'

func AI_Move():
	$Timer.set_wait_time(1)
	$Timer.start()

func _on_Button1_pressed():
	if (tt2['1'] != 'X' and tt2['1'] != 'O'):
		tt2['1'] = 'X'
		AI.erase('1')
		player = '2'
		moves += 1
		get_node("ColorRect/Button1").text = tt2['1']
		check()
		AI_Move()

func _on_Button2_pressed():
	if (tt2['2'] != 'X' and tt2['2'] != 'O'):
		tt2['2'] = 'X'
		AI.erase('2')
		player = '2'
		moves += 1
		get_node("ColorRect/Button2").text = tt2['2']
		check()
		AI_Move()

func _on_Button3_pressed():
	if (tt2['3'] != 'X' and tt2['3'] != 'O'):
		tt2['3'] = 'X'
		AI.erase('3')
		player = '2'
		moves += 1
		get_node("ColorRect/Button3").text = tt2['3']
		check()
		AI_Move()

func _on_Button4_pressed():
	if (tt2['4'] != 'X' and tt2['4'] != 'O'):
		tt2['4'] = 'X'
		AI.erase('4')
		player = '2'
		moves += 1
		get_node("ColorRect/Button4").text = tt2['4']
		check()
		AI_Move()

func _on_Button5_pressed():
	if (tt2['5'] != 'X' and tt2['5'] != 'O'):
		tt2['5'] = 'X'
		AI.erase('5')
		player = '2'
		moves += 1
		get_node("ColorRect/Button5").text = tt2['5']
		check()
		AI_Move()

func _on_Button6_pressed():
	if (tt2['6'] != 'X' and tt2['6'] != 'O'):
		tt2['6'] = 'X'
		AI.erase('6')
		player = '2'
		moves += 1
		get_node("ColorRect/Button6").text = tt2['6']
		check()
		AI_Move()

func _on_Button7_pressed():
	if (tt2['7'] != 'X' and tt2['7'] != 'O'):
		tt2['7'] = 'X'
		AI.erase('7')
		player = '2'
		moves += 1
		get_node("ColorRect/Button7").text = tt2['7']
		check()
		AI_Move()

func _on_Button8_pressed():
	if (tt2['8'] != 'X' and tt2['8'] != 'O'):
		tt2['8'] = 'X'
		AI.erase('8')
		player = '2'
		moves += 1
		get_node("ColorRect/Button8").text = tt2['8']
		check()
		AI_Move()

func _on_Button9_pressed():
	if (tt2['9'] != 'X' and tt2['9'] != 'O'):
		tt2['9'] = 'X'
		AI.erase('9')
		player = '2'
		moves += 1
		get_node("ColorRect/Button9").text = tt2['9']
		check()
		AI_Move()

func _on_ButtonRestart_pressed():
	get_tree().change_scene("res://scenes/TicTacToe.tscn")

func _on_ButtonExit_pressed():
	get_tree().quit()


func TimerTimeout():
	if AI.size() != 0:
		var random = AI[randi() % AI.size()]
		tt2[random] = 'O'
		AI.erase(random)
		player = '1'
		moves += 1
		get_node("ColorRect/Button"+random).text = tt2[random]
		check()
		$Timer.stop()
	else:
		pass
