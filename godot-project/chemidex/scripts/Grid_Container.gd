extends Control

export (NodePath) var drop_path
onready var dropdown = get_node(drop_path)
var chemistry_array = ["Iron", "Magnesium", "Oxygen", "Chlorine", "Carbon"]

func _ready():
	dropdown.connect("item_selected", self, "on_item_selected")
	add_Chemistry_items()
	select_item(4)

func add_items():
	dropdown.add_item("Item 1")
	dropdown.add_separator()
	dropdown.add_item("Item 2")
	dropdown.add_separator()
	dropdown.add_item("Item 3")
	dropdown.add_separator()
	dropdown.add_item("Item 4")
	dropdown.add_separator()
	dropdown.add_item("Item 5")
	dropdown.add_separator()

func add_Chemistry_items():
	for item in chemistry_array:
		dropdown.add_item(item)

func select_item(id):
	dropdown.select(id)

func on_item_selected(id):
	print(chemistry_array[id])
