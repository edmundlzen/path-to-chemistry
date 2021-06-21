extends Spatial

var arkit = null

func _ready():
	arkit = ARVRServer.find_interface("ARKit")
	if arkit:
		if arkit.initialize():
			get_viewport().arvr = true
			arkit.ar_is_anchor_detection_enabled = true
			
func _input(event):
	var anchor = get_node("ARVROrigin/ARVRAnchor")
	if (event.is_class("InputEventMouseButton") and event.pressed and anchor.get_is_active()):
		var camera = get_node("ARVROrigin/ARVRCamera")
		var from = camera.project_ray_origin(event.position)
		var direction = camera.project_ray_normal(event.position)
		var plane = Plane(anchor.translation, anchor.translation + anchor.transform.basis.x, anchor.translation + anchor.translation.basis.Z)
		var intersect = plane.intersects_ray(from, direction)
		if intersect:
			$MeshInstance.translation = intersect
			$MeshInstance.visible = true
