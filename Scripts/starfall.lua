local colors = {"#FF71CE", "#01CDFE", "#05FFA1", "#B967FF", "#FFFB96"}

function draw_star(size)
    turtle.pen_down()
    for i = 1, 5 do
        turtle.move(size)
        turtle.turn(144)
    end
    turtle.pen_up()
end

turtle.pen_up()
for i = 1, 365 do
    local x = math.random(-300, 300)
    local y = math.random(-300, 300)
    
    turtle.turn(math.random(0, 360))
    turtle.move(math.random(50, 150))
    
    turtle.color(colors[math.random(1, #colors)])
    draw_star(math.random(5, 20))
end