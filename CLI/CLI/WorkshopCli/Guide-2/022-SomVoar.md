[color=white]
Para melhorar o jogo vamos adicionar um som, para quando o pássaro bater as asas. 

1. Dentro da função "load()", adiciona o seguinte:

[/color] [color=blue]
    somAsa = love.audio.newSource("flap.wav", "static")
[/color] [color=white]

2. Agora para tocar o som, na função "update(dt)",na parte onde verificamos se a tecla 
"espaço" foi pressionada, adiciona em baixo desta linha "passaro.velocidade = passaro.salto" 
o seguinte:

[/color] [color=blue]
    somAsa:play()
[/color] [color=white]

Agora quando tocares no chão o jogo acaba, e também já não podes subir infinitamente.
[/color] 