extends Node2D

var Question = ''
var WordCount = 0

func Button0():
	if WordCount <= 83:
		Question += '0'
		WordCount += 1
		get_node("Label").text = Question
	else:
		pass

func Button1():
	if WordCount <= 83:
		Question += '1'
		WordCount += 1
		get_node("Label").text = Question
	else:
		pass

func Button2():
	if WordCount <= 83:
		Question += '2'
		WordCount += 1
		get_node("Label").text = Question
	else:
		pass

func Button3():
	if WordCount <= 83:
		Question += '3'
		WordCount += 1
		get_node("Label").text = Question
	else:
		pass

func Button4():
	if WordCount <= 83:
		Question += '4'
		WordCount += 1
		get_node("Label").text = Question
	else:
		pass

func Button5():
	if WordCount <= 83:
		Question += '5'
		WordCount += 1
		get_node("Label").text = Question
	else:
		pass

func Button6():
	if WordCount <= 83:
		Question += '6'
		WordCount += 1
		get_node("Label").text = Question
	else:
		pass

func Button7():
	if WordCount <= 83:
		Question += '7'
		WordCount += 1
		get_node("Label").text = Question
	else:
		pass

func Button8():
	if WordCount <= 83:
		Question += '8'
		WordCount += 1
		get_node("Label").text = Question
	else:
		pass

func Button9():
	if WordCount <= 83:
		Question += '9'
		WordCount += 1
		get_node("Label").text = Question
	else:
		pass

func ButtonMultiply():
	if WordCount <= 83:
		Question += '*'
		WordCount += 1
		get_node("Label").text = Question
	else:
		pass

func ButtonMinus():
	if WordCount <= 83:
		Question += '-'
		WordCount += 1
		get_node("Label").text = Question
	else:
		pass

func ButtonPlus():
	if WordCount <= 83:
		Question += '+'
		WordCount += 1
		get_node("Label").text = Question
	else:
		pass

func ButtonDivide():
	if WordCount <= 83:
		Question += '/'
		WordCount += 1
		get_node("Label").text = Question
	else:
		pass

func ButtonDot():
	if WordCount <= 83:
		Question += '.'
		WordCount += 1
		get_node("Label").text = Question
	else:
		pass

func ButtonClear():
	Question = ''
	WordCount = 0
	get_node("Label").text = Question

func ButtonBackspace():
	var List = []
	for Quest in Question:
		List.append(Quest)
	List.remove(WordCount - 1)
	WordCount -= 1
	Question = ''
	for Ans in List:
		Question += Ans
	get_node("Label").text = Question
	
func ButtonPercentage():
	if WordCount <= 83:
		Question += '/(100)'
		WordCount += 1
		get_node("Label").text = Question
	else:
		pass

func ButtonEqual():
	var expression = Expression.new()
	expression.parse(str(Question))
	Question = ''
	Question += str(expression.execute())
	get_node("Label").text = Question
