[color=white]
Para definir a posição de algo no ecrã recorremos ao sistema de
coordenadas do gráfico cartesiano. No entanto, em programação
é um pouco diferente, o eixo do Y aumenta para baixo em vez
de para cima.

-------------->x
|
|
|
|
V Y

Agora que temos o fundo, vamos adicionar o personagem 
principal do jogo: o pássaro!

1. Adiciona na função "load()" as variáveis do pássaro:
[/color] [color=blue]
   -- Variáveis do Pássaro
   birdImage = love.graphics.newImage("bird.png")
    bird = {
        x = 100,
        y = 200,
        width = 40,
        height = 30,
        speed = 0,
        gravity = 0.25,
        lift = -4
    }
[/color] [color=white]
Aqui definimos o tamanho do pássaro (width = 40, height = 30) 
e que ele será afetado pela gravidade!

2. Agora, na função "love.draw()", desenha o pássaro no 
tamanho correto:
[/color] [color=blue]
   love.graphics.draw(
   birdImage,
   bird.x,
   bird.y,
   0,
   bird.width / birdImage:getWidth(),
   bird.height / birdImage:getHeight()
   )

[/color] [color=white]
Executa o jogo (Alt+L) e verás o pássaro no ecrã!
[/color] [color=red]
     ____  _____ ____    _    _____ ___ ___  
    |  _ \| ____/ ___|  / \  |  ___|_ _/ _ \
    | | | |  _| \___ \ / _ \ | |_   | | | | |
    | |_| | |___ ___) / ___ \|  _|  | | |_| |
    |____/|_____|____/_/   \_\_|   |___\___/
[/color] [color=white]

Muda a posição inicial do pássaro na variavel bird (y = 200) para 150 e vê 
onde ele começa na tela!

Volta a executar o jogo (Alt+L) para verificar se está tudo bem.
[/color]
