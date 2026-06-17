function love.load() -- Inicio da função "load()"
    local ecraLargura, ecraAltura = love.window.getDesktopDimensions()
    local janelaLargura = math.min(1000, ecraLargura - 80)
    local janelaAltura = math.min(700, ecraAltura - 120)
    love.window.setMode(janelaLargura, janelaAltura)


end -- Fim da função "load()"


function love.draw() -- Inicio da função "draw()"
    love.graphics.print("Ola mundo!", 200, 200);


end -- Fim da função "draw()"

function love.update(dt) -- Inicio da função "update()"

end -- Fim da função "update()"
