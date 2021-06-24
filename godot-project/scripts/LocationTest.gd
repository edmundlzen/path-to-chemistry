extends Node2D


var singleton = null


# Called when the node enters the scene tree for the first time.
func _ready():
	get_tree().connect("on_request_permissions_result", self, "result")
	if (OS.request_permissions()):
		print("permissions ok")
		result("", true)

func _exit_tree():
	if singleton != null:
		singleton.stopLocationUpdates()

func on_location_updates(updates: Dictionary):
	get_node("currentLongitude").text = "Longitude: " + String(updates["longitude"])
	get_node("currentLatitude").text = "Latitude: " + String(updates["latitude"])

func on_last_known_location(last_known_location: Dictionary):
	print("Location Updates: ", last_known_location)

func on_location_error(errorCode: int, message: String):
	print("Error Code: ", errorCode, " Message: ", message)

func _on_startupdate_pressed():
	singleton.startLocationUpdates(100.0, 50.0)

func _on_stopupdate_pressed():
	singleton.stopLocationUpdates()

func result(permission: String, granted: bool):
	if Engine.has_singleton("LocationPlugin"):
		singleton = Engine.get_singleton("LocationPlugin")
		singleton.connect("onLocationUpdates", self, "on_location_updates")
		singleton.connect("onLastKnownLocation", self, "on_last_known_location")
		singleton.connect("onLocationError", self, "on_location_error")
