
Para criar um jogo, precisamos de recursos como imagens, som e fontes.
Vamos começar por carregar as imagens.

1. Adicionamos várias imagens dentro da pasta "mygame", com os nomes de nave, inimigos e mísseis que usarás no jogo clica para ver as imagens.

Agora vamos desenhar o jogador!

1. Temos de primeiro carregar a imagem, e depois desenha-la.

2. Primeiro escolhe uma imagem para a nave.
3. Depois, para carregar a imagem,
   escreve `shipImage = love.graphics.newImage("nome-imagem.png")` dentro da funcao load(), na linha anterior à palavra end

3. Nao te esqueças de alterar "nome-imagem" pelo nome da imagem que tu escolheste anteriormente.

4. Depois, escreve `love.graphics.draw(shipImage, 100, 100, 0, 1, 1)` na funcao draw.

Quando vires a imagem na tela de jogo, continua para o próximo passo.
