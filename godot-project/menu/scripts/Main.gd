extends Node2D

onready var Credits = load("res://menu/scenes/Credits.tscn")

func _ready():
	pass


func _on_Lab_pressed():
	get_tree().change_scene("res://lab/scenes/Lab.tscn")

func _on_Save_pressed():
	get_tree().change_scene("res://chemidex/scenes/Grid_Container.tscn")

func _on_Exit_pressed():
	get_tree().quit()
	
func _on_Credits_pressed():
	add_child(Credits.instance())
