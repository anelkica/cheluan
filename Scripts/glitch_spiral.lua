local x = 1

while x < 400 do
    local r = math.random(0, 255)
    local g = math.random(0, 255)
    local b = math.random(0, 255)

    turtle.color(r, g, b)
    turtle.move(50 + x)
    turtle.turn(90.991)

    x = x + 1
end