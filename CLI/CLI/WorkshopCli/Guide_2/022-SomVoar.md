[color=white]
O jogo está quase completo, para melhorar o jogo vamos adicionar um som, para quando o pássaro voar. 

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

Agora sempre que pressionares a tecla "espaço", ele vai tocar um som.
[/color] 