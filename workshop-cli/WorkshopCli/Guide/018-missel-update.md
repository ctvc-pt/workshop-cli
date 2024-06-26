
Agora vamos fazer com que os mísseis se movam e não fiquem parados quando disparas. 

1. Na função "load()", adiciona nas variáveis do míssil:

   missilVelocidade = 250
   podeDisparar = true

2. No fim do teu código todo, adiciona esta função:

   -- Função para atualizar os mísseis
   function atualizarMisseis(dt)
        for i=#misseis, 1, -1 do
            missil = misseis[i]
            missil.posicaoX = missil.posicaoX + dt * missil.velocidade
        end
   end

3. Agora vamos alterar o código que adicionamos no último passo na função "update(dt)" (substitui o código que faz disparar os misseis):

    if love.keyboard.isDown("space") then
        if podeDisparar then
            missil = {
                posicaoX = posicaoX + 64, 
                posicaoY = posicaoY + 32, 
                width = 16, height=16, 
                velocidade = missilVelocidade, 
                imagem = missilImagem}
            table.insert(misseis, missil)
        end
    end
    atualizarMisseis(dt)

O código inserido agora faz o seguinte:
 - Se a tecla "espaço" for pressionada, um míssil é disparado com base na velocidade do jogador. A velocidade do míssil é ajustada se o jogador estiver a mover-se para a esquerda ou direita. O missil também é definido aqui.
 - A função "atualizarMisseis(dt)" é chamada para atualizar o movimento dos mísseis existentes no jogo.

Agora podes iniciar o teu jogo e divertir-te um pouco.
