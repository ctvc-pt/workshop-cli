[color=white]
Para dar mais dificuldade ao jogo vamos criar os obstáculos.

1. Dentor da função "load()" adiciona o seguinte código:
[/color] [color=blue]
    pipes = {}
    pipeWidth = 50
    pipeGap = 150
    pipeSpeed = 2
    pipeSpawnTime = 2
    pipeTimer = 0 

[/color] [color=white]

Aqui definimos a largura dos canos (pipeWidth = 50), o espaço 
entre eles (pipeGap = 150) e a velocidade (pipeSpeed = 2).

2. Dentor da função "update(dt)" faz os canos moverem-se para a esquerda:
[/color] [color=blue]
    for i, pipe in ipairs(pipes) do
        pipe.x = pipe.x - pipeSpeed
    end 
    pipeTimer = pipeTimer + dt
    if pipeTimer >= pipeSpawnTime then
    local pipeHeight = love.math.random(50, love.graphics.getHeight() - pipeGap - 50)
        table.insert(pipes, {x = love.graphics.getWidth(), y = pipeHeight})
        pipeTimer = 0
    end

[/color] [color=white]

Ainda não vês os canos? Não te preocupes, agora vamos desenhá-los!

3. Dentor da função "draw()" adiciona:
[/color] [color=blue]
love.graphics.setColor(0, 0.5, 0)
    for i, pipe in ipairs(pipes) do
        love.graphics.rectangle("fill", pipe.x, 0, pipeWidth, pipe.y)
        love.graphics.rectangle("fill", pipe.x, pipe.y + pipeGap, pipeWidth, 
        love.graphics.getHeight() - pipe.y - pipeGap)
    end
love.graphics.setColor(1, 1, 1)

[/color] [color=white]

Agora os canos aparecem e movem-se pela tela!

[/color] [color=red]
     ____  _____ ____    _    _____ ___ ___  
    |  _ \| ____/ ___|  / \  |  ___|_ _/ _ \
    | | | |  _| \___ \ / _ \ | |_   | | | | |
    | |_| | |___ ___) / ___ \|  _|  | | |_| |
    |____/|_____|____/_/   \_\_|   |___\___/
[/color] [color=white]

Aumenta o valor de pipeWidth para 80 e vê o que acontece.
[/color] 