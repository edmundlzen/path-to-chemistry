extends CanvasLayer

func Resume():
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	queue_free()
	get_tree().paused = false

func Restart():
	get_tree().change_scene("res://lab/scenes/Lab.tscn")
	get_tree().paused = false

func Main():
	get_tree().change_scene("res://menu/scenes/Menu.tscn")
	get_tree().paused = false
