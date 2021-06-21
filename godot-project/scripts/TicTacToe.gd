extends Node2D

var tt2 = {
	'1':'1','2':'2','3':'3',
	'4':'4','5':'5','6':'6',
	'7':'7','8':'8','9':'9'
}
var AI = ['1','2','3','4','5','6','7','8','9']
var player = '1'
var moves = 0
var random = str(randi()%AI.size())

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
	if (tt2['1'] == 'O' and tt2['2'] == 'O' and tt2['3'] == 'O'):
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

func _on_Button1_pressed():
	if (tt2['1'] != 'X' and tt2['1'] != 'O'):
		if (player == '1'):
			tt2['1'] = 'X'
			player = '2'
		elif (player == '2'):
			tt2['1'] = 'O'
			player = '1'
		moves += 1
		get_node("ColorRect/Button1").text = tt2['1']
		check()
		
func _on_Button2_pressed():
	if (tt2['2'] != 'X' and tt2['2'] != 'O'):
		if (player == '1'):
			tt2['2'] = 'X'
			player = '2'
		elif (player == '2'):
			tt2['2'] = 'O'
			player = '1'
		moves += 1
		get_node("ColorRect/Button2").text = tt2['2']
		check()
		
func _on_Button3_pressed():
	if (tt2['3'] != 'X' and tt2['3'] != 'O'):
		if (player == '1'):
			tt2['3'] = 'X'
			player = '2'
		elif (player == '2'):
			tt2['3'] = 'O'
			player = '1'
		moves += 1
		get_node("ColorRect/Button3").text = tt2['3']
		check()

func _on_Button4_pressed():
	if (tt2['4'] != 'X' and tt2['4'] != 'O'):
		if (player == '1'):
			tt2['4'] = 'X'
			player = '2'
		elif (player == '2'):
			tt2['4'] = 'O'
			player = '1'
		moves += 1
		get_node("ColorRect/Button4").text = tt2['4']
		check()
		
func _on_Button5_pressed():
	if (tt2['5'] != 'X' and tt2['5'] != 'O'):
		if (player == '1'):
			tt2['5'] = 'X'
			player = '2'
		elif (player == '2'):
			tt2['5'] = 'O'
			player = '1'
		moves += 1
		get_node("ColorRect/Button5").text = tt2['5']
		check()
		
func _on_Button6_pressed():
	if (tt2['6'] != 'X' and tt2['6'] != 'O'):
		if (player == '1'):
			tt2['6'] = 'X'
			player = '2'
		elif (player == '2'):
			tt2['6'] = 'O'
			player = '1'
		moves += 1
		get_node("ColorRect/Button6").text = tt2['6']
		check()
		
func _on_Button7_pressed():
	if (tt2['7'] != 'X' and tt2['7'] != 'O'):
		if (player == '1'):
			tt2['7'] = 'X'
			player = '2'
		elif (player == '2'):
			tt2['7'] = 'O'
			player = '1'
		moves += 1
		get_node("ColorRect/Button7").text = tt2['7']
		check()
		
func _on_Button8_pressed():
	if (tt2['8'] != 'X' and tt2['8'] != 'O'):
		if (player == '1'):
			tt2['8'] = 'X'
			player = '2'
		elif (player == '2'):
			tt2['8'] = 'O'
			player = '1'
		moves += 1
		get_node("ColorRect/Button8").text = tt2['8']
		check()
		
func _on_Button9_pressed():
	if (tt2['9'] != 'X' and tt2['9'] != 'O'):
		if (player == '1'):
			tt2['9'] = 'X'
			player = '2'
		elif (player == '2'):
			tt2['9'] = 'O'
			player = '1'
		moves += 1
		get_node("ColorRect/Button9").text = tt2['9']
		check()
		
func _on_ButtonRestart_pressed():
	get_tree().change_scene("res://AR/TicTacToe.tscn")

func _on_ButtonExit_pressed():
	get_tree().quit()
