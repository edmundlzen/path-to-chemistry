extends KinematicBody

onready var Pause = load("res://scenes/Pause.tscn")
export var Speed = 10
export var Acceleration = 5
export var Gravity = 1
export var jumpForce = 30
export var Sensitivity = 0.3
var Velocity = Vector3()
var Rotation = 0

func _ready():
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func _input(event):
	if event is InputEventMouseMotion:
		checkCollide()
		$Head.rotate_y(deg2rad(-event.relative.x * Sensitivity))
		var Delta = event.relative.y * Sensitivity
		$Head/Camera.rotate_x(deg2rad(-Delta))
		Rotation += Delta

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
	elif Input.is_action_pressed("Forward"):
		Direction -= headBasis.y
	elif Input.is_action_pressed("Backward"):
		Direction += headBasis.y
	if Input.is_action_pressed("Left") and Input.is_action_pressed("Right"):
		Direction = Vector3(0,0,0)
	elif Input.is_action_pressed("Left"):
		Direction -= headBasis.x
	elif Input.is_action_pressed("Right"):
		Direction += headBasis.x
	Direction = Direction.normalized()
	Velocity = Velocity.linear_interpolate(Direction * Speed, Acceleration * delta)
	Velocity.y -= Gravity
	if Input.is_action_just_pressed("Jump") and is_on_floor():
		Velocity.y += jumpForce
	Velocity = move_and_slide(Velocity, Vector3.UP)

func checkCollide():
	if $Head/Camera/RayCast.is_colliding():
		var Collider = $Head/Camera/RayCast.get_collider()
		if Collider.is_in_group('Item'):
			$Label.text = 'Magnesium'
