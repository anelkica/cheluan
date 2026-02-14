local scale = 1    -- increase to zoom in
local growth = 0.35 
local length = 0   

for i = 1, 2000 do
    length = length + (growth / 360) 
    turtle.move(length * scale)
    turtle.turn(1)
end