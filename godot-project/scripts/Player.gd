extends KinematicBody

onready var Pause = load("res://scenes/Pause.tscn")
export var Speed = 10
export var Gravity = 1
export var jumpForce = 30
export var Acceleration = 5
export var Sensitivity = 0.3
var Velocity = Vector3()
var Scroll = 1
var Rotation = 0
var Style = StyleBoxFlat.new()
var Original = StyleBoxFlat.new()
var Pickable = false
var Store = {
		'Slot1':'','Slot2':'','Slot3':'',
		'Slot4':'K','Slot5':'','Slot6':'',
		'Slot7':'Water','Slot8':''
		}

func _ready():
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	Style.set_corner_radius_all(8)
	Style.set_bg_color(Color("#17e9a9"))
	Original.set_corner_radius_all(8)
	Original.set_bg_color(Color("#4b4b4b"))
	get_node("Hotbar/GridContainer/Panel"+str(Scroll)).set('custom_styles/panel', Style)
	
func _input(event):
	if event is InputEventMouseMotion:
		$Head.rotate_y(deg2rad(-event.relative.x * Sensitivity))
		var Delta = event.relative.y * Sensitivity
		if Rotation + Delta > -90 and Rotation + Delta < 90:    
			$Head/Camera.rotate_x(deg2rad(-Delta))
			Rotation += Delta
	if Input.is_action_pressed("scrollUp"):
		if Scroll < 8:
			get_node("Hotbar/GridContainer/Panel"+str(Scroll)).set('custom_styles/panel', Original)
			Scroll += 1
			get_node("Hotbar/GridContainer/Panel"+str(Scroll)).set('custom_styles/panel', Style)
		else:
			get_node("Hotbar/GridContainer/Panel8").set('custom_styles/panel', Original)
			Scroll = 1
			get_node("Hotbar/GridContainer/Panel"+str(Scroll)).set('custom_styles/panel', Style)
	if Input.is_action_pressed("scrollDown"):
		if Scroll > 1:
			get_node("Hotbar/GridContainer/Panel"+str(Scroll)).set('custom_styles/panel', Original)
			Scroll -= 1
			get_node("Hotbar/GridContainer/Panel"+str(Scroll)).set('custom_styles/panel', Style)
		else:
			get_node("Hotbar/GridContainer/Panel1").set('custom_styles/panel', Original)
			Scroll = 8
			get_node("Hotbar/GridContainer/Panel"+str(Scroll)).set('custom_styles/panel', Style)

func _process(_delta):
	if Input.is_action_just_pressed("Escape"):
		add_child(Pause.instance())
		get_tree().paused = true
		Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
	if $Head/Camera/RayCast.is_colliding():
		var Collider = $Head/Camera/RayCast.get_collider()
		if Collider.is_in_group("Element"):
			$Label.text = 'Magnesium'
			Pickable = true
			$Crosshair.texture = load("res://assets/Hand.png")
	else:
		$Label.text = ''
		Pickable = false
		$Crosshair.texture = load("res://assets/Crosshair.png")
	if Input.is_action_just_pressed("leftMouse"):
		if Pickable == true:
			var Collider = $Head/Camera/RayCast.get_collider()
			if Collider.is_in_group('Element'):
				if Store['Slot'+str(Scroll)] == '':
					Collider.queue_free()
					Store['Slot'+str(Scroll)] = 'Mg'
					print(Store['Slot'+str(Scroll)])
					get_node("Hotbar/GridContainer/Panel"+str(Scroll)+"/Sprite").texture = load("res://assets/"+Store['Slot'+str(Scroll)]+".png")
					get_node("Head/Camera/Sprite3D").texture = load("res://assets/"+Store['Slot'+str(Scroll)]+".png")
					$Crosshair.texture = load("res://assets/Crosshair.png")

func _physics_process(delta):
	var headBasis = $Head.get_global_transform().basis
	var Direction = Vector3()
	if Input.is_action_pressed("Forward") and Input.is_action_pressed("Backward"):
		Direction = Vector3(0, 0, 0)
	elif Input.is_action_pressed("Forward"):
		Direction -= headBasis.y
	elif Input.is_action_pressed("Backward"):
		Direction += headBasis.y
	if Input.is_action_pressed("Left") and Input.is_action_pressed("Right"):
		Direction = Vector3(0, 0, 0)
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
