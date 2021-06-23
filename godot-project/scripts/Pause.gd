extends CanvasLayer

func Resume():
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	queue_free()
	get_tree().paused = false

func Quit():
	get_tree().quit()
