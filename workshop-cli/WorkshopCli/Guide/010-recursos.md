
Para criar um jogo, precisamos de recursos como imagens, som e fontes.
Vamos começar por carregar as imagens.

1. Adicionei várias imagens dentro da pasta "mygame", com os nomes de nave, inimigos e mísseis que usarás no jogo.
Podes clicar nas imagens para as ver.


Agora vamos desenhar o jogador.

Temos de primeiro carregar a imagem, e depois desenha-la.

1. Para carregar a imagem, escreve:
   imagem = love.graphics.newImage("nome-imagem.png")
   dentro da funcao "load()".

Não te esqueças de alterar "nome-imagem" pelo nome da nave que tu escolheste anteriormente.

2. Depois, escreve:
   love.graphics.draw(imagem, 100, 100, 0, 1, 1)
   na função "draw()".

Quando vires a imagem na tela de jogo, continua para o próximo passo (alt+L).
