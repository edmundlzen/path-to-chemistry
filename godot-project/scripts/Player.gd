extends KinematicBody

onready var Pause = load("res://scenes/Pause.tscn")
export var Speed = 10
export var Acceleration = 5
export var Gravity = 1
export var jumpForce = 30
export var Sensitivity = 0.3
var Velocity = Vector3()
var Rotation = 0
var Score = 1
var Style = StyleBoxFlat.new()
var Original = StyleBoxFlat.new()

func _ready():
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	Style.set_bg_color(Color("#17e9a9"))
	Style.set_corner_radius_all(8)
	Original.set_bg_color(Color("#4b4b4b"))
	Original.set_corner_radius_all(8)
	get_node("Hotbar/GridContainer/Panel"+str(Score)).set('custom_styles/panel', Style)

func _input(event):
	if event is InputEventMouseMotion:
		checkCollide()
		$Head.rotate_y(deg2rad(-event.relative.x * Sensitivity))
		var Delta = event.relative.y * Sensitivity
		$Head/Camera.rotate_x(deg2rad(-Delta))
		Rotation += Delta
	if Input.is_action_pressed("Scroll_Up"):
		if Score < 8:
			Score += 1
			get_node("Hotbar/GridContainer/Panel"+str(Score)).set('custom_styles/panel', Style)
			get_node("Hotbar/GridContainer/Panel"+str(Score-1)).set('custom_styles/panel', Original)
		else:
			Score = 1
			get_node("Hotbar/GridContainer/Panel"+str(Score)).set('custom_styles/panel', Style)
			get_node("Hotbar/GridContainer/Panel8").set('custom_styles/panel', Original)
	elif Input.is_action_pressed("Scroll_Down"):
		if Score > 1:
			Score -= 1
			get_node("Hotbar/GridContainer/Panel"+str(Score)).set('custom_styles/panel', Style)
			get_node("Hotbar/GridContainer/Panel"+str(Score+1)).set('custom_styles/panel', Original)
		else:
			Score = 8
			get_node("Hotbar/GridContainer/Panel"+str(Score)).set('custom_styles/panel', Style)
			get_node("Hotbar/GridContainer/Panel1").set('custom_styles/panel', Original)

func _process(_delta):
	if Input.is_action_just_pressed("Escape"):
		add_child(Pause.instance())
		get_tree().paused = true
		Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)

func _physics_process(delta):
	var headBasis = $Head.get_global_transform().basis
	var Direction = Vector3()
	if Input.is_action_pressed("Forward") and Input.is_action_pressed("Backward"):
		Direction = Vector3(0,0,0)
		checkCollide()
	elif Input.is_action_pressed("Forward"):
		Direction -= headBasis.y
		checkCollide()
	elif Input.is_action_pressed("Backward"):
		Direction += headBasis.y
		checkCollide()
	if Input.is_action_pressed("Left") and Input.is_action_pressed("Right"):
		Direction = Vector3(0,0,0)
		checkCollide()
	elif Input.is_action_pressed("Left"):
		Direction -= headBasis.x
		checkCollide()
	elif Input.is_action_pressed("Right"):
		Direction += headBasis.x
		checkCollide()
	Direction = Direction.normalized()
	Velocity = Velocity.linear_interpolate(Direction * Speed, Acceleration * delta)
	Velocity.y -= Gravity
	if Input.is_action_just_pressed("Jump") and is_on_floor():
		Velocity.y += jumpForce
		checkCollide()
	Velocity = move_and_slide(Velocity, Vector3.UP)

func checkCollide():
	if $Head/Camera/RayCast.is_colliding():
		var Collider = $Head/Camera/RayCast.get_collider()
		if Collider.is_in_group('Item'):
			$Label.text = 'Magnesium'
	else:
		$Label.text = ''
