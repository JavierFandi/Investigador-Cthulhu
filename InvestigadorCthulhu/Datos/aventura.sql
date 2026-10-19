-- =====================================================================
--  aventura.sql  ·  Tablas de juego + aventura "La última carta del profesor Ward"
--  Se ejecuta una sola vez: el programa lo corre si la tabla 'escenas' no existe.
-- =====================================================================

-- ---------- Investigadores creados por los jugadores ----------
CREATE TABLE IF NOT EXISTS investigadores (
  id           INTEGER PRIMARY KEY AUTOINCREMENT,
  nombre       TEXT NOT NULL,
  ocupacion_id INTEGER REFERENCES ocupaciones(id),
  edad         INTEGER,
  fue INTEGER, con INTEGER, tam INTEGER, des INTEGER,
  apa INTEGER, intel INTEGER, pod INTEGER, edu INTEGER,
  suerte INTEGER, pv INTEGER, cordura INTEGER, pm INTEGER,
  mov INTEGER, bd TEXT, corpulencia INTEGER, credito INTEGER,
  creado TEXT DEFAULT (datetime('now','localtime'))
);

CREATE TABLE IF NOT EXISTS investigador_habilidades (
  investigador_id INTEGER NOT NULL REFERENCES investigadores(id) ON DELETE CASCADE,
  habilidad       TEXT NOT NULL,
  valor           INTEGER NOT NULL,
  PRIMARY KEY (investigador_id, habilidad)
);

-- ---------- Motor de la historia ----------
CREATE TABLE IF NOT EXISTS escenas (
  codigo          TEXT PRIMARY KEY,           -- 'INICIO', 'ESTUDIO', ...
  titulo          TEXT NOT NULL,
  texto           TEXT NOT NULL,
  tirada_cordura  TEXT,                       -- formato 'éxito/fallo', p. ej. '0/1D4'
  es_final        INTEGER NOT NULL DEFAULT 0,
  tipo_final      TEXT                        -- victoria, rescate, huida, muerte, locura
);

CREATE TABLE IF NOT EXISTS opciones (
  id            INTEGER PRIMARY KEY AUTOINCREMENT,
  escena_codigo TEXT NOT NULL REFERENCES escenas(codigo),
  orden         INTEGER NOT NULL,
  texto         TEXT NOT NULL,
  habilidad     TEXT,                         -- NULL = sin tirada. Puede ser una habilidad o FUE/CON/DES/POD/SUERTE...
  dificultad    TEXT NOT NULL DEFAULT 'Normal', -- Normal / Difícil / Extrema
  escena_exito  TEXT NOT NULL REFERENCES escenas(codigo),
  escena_fallo  TEXT REFERENCES escenas(codigo),
  texto_exito   TEXT,
  texto_fallo   TEXT,
  dano_fallo    TEXT,                         -- dados de daño si falla, p. ej. '1D6'
  cordura_fallo TEXT                          -- dados de Cordura perdida si falla
);

-- ---------- Escenas ----------
INSERT INTO escenas (codigo, titulo, texto, tirada_cordura, es_final, tipo_final) VALUES
('INICIO', 'Una carta desde Haverford',
 'Octubre de 1925. Hace una semana recibiste una carta de tu viejo amigo, el profesor Elias Ward: «Si el viernes no tenés noticias mías, vení a Haverford. No confíes en nadie del molino». Hoy es sábado. Llueve sobre el pueblo costero y la casa de Ward, al final de Ash Street, está a oscuras. La puerta principal tiene llave. En la casa de al lado, una cortina se mueve.',
 NULL, 0, NULL),

('VECINA', 'La señora Pell',
 'La señora Pell te hace pasar a una cocina que huele a té y a querosén. Baja la voz: «El profesor salía todas las noches con un farol hacia el viejo molino de los Hollis, cargado de libros. El jueves no volvió». Busca en un cajón y te da una llave de bronce. «La dejó por si le pasaba algo. Dijo que usted vendría».',
 NULL, 0, NULL),

('CASA_EXTERIOR', 'Detrás de la casa',
 'Rodeás la casa por un jardín invadido de algas secas, cosa rara tan lejos del mar. La puerta de la cocina está cerrada con llave. Arriba, la ventana del estudio da a un pequeño balcón de madera.',
 NULL, 0, NULL),

('ESTUDIO', 'El estudio del profesor',
 'El estudio está revuelto: cajones abiertos, mapas de la costa clavados en la pared, velas consumidas. Todo huele a salmuera. Sobre el escritorio hay pilas de papeles y libros, y en el piso, la alfombra está corrida, como si alguien la hubiera arrastrado con prisa.',
 NULL, 0, NULL),

('ESTUDIO_TARDE', 'Se hace de noche',
 'Pasan las horas entre papeles inútiles. Cuando levantás la vista, ya es de noche. Entonces lo oís: un rasguño lento, húmedo, que viene de debajo del piso.',
 '0/1', 0, NULL),

('DIARIO', 'El diario de Ward',
 'Entre los libros encontrás el diario del profesor. Las últimas páginas hablan de un manuscrito hallado en el molino, el «De Profundis Marinis», y de un sello de piedra bajo la rueda hidráulica: «Con la marea alta del sábado, los Hollis van a abrir el Ojo que duerme bajo el agua». La última entrada termina en un párrafo en latín, escrito con letra temblorosa. Al leerlo sentís frío en la nuca.',
 '0/1D2', 0, NULL),

('DIARIO_TRADUCIDO', 'Las palabras del sello',
 'Con paciencia traducís el latín. Ward descubrió cómo cerrar el sello: «sal de la tierra sobre la piedra y las palabras dichas al revés». En la cocina encontrás un frasco de sal gruesa. Lo guardás en el bolsillo junto con la página arrancada. La marea alta es esta noche.',
 NULL, 0, NULL),

('TRAMPILLA', 'Bajo la alfombra',
 'Bajo la alfombra hay una trampilla. Al abrirla te golpea un aire helado y salobre. Una escalera de piedra baja hacia un túnel que corre en dirección al río. En las paredes hay marcas de dedos mojados, demasiado largos para ser humanos.',
 '0/1D3', 0, NULL),

('TUNEL', 'El túnel',
 'El túnel avanza bajo el río. El agua te llega a los tobillos. Al final, una escalera de madera sube hacia una luz anaranjada y un canto grave, repetido, que hace vibrar las piedras: estás bajo el molino de los Hollis.',
 NULL, 0, NULL),

('TUNEL_CRIATURA', 'Algo en el agua',
 'A mitad del túnel el agua se agita. Algo pálido y resbaladizo se enrosca en tu pierna y tira hacia la oscuridad. Durante un instante ves un ojo enorme, sin párpado, que te mira desde abajo.',
 '1/1D6', 0, NULL),

('MOLINO_CAMINO', 'El camino al molino',
 'Seguís el sendero del río bajo la lluvia. El molino de los Hollis se recorta contra el cielo, con luces que se mueven detrás de las ventanas tapiadas. Desde adentro llega un canto en una lengua que no reconocés.',
 NULL, 0, NULL),

('MOLINO_PREPARADO', 'El camino al molino (con la sal)',
 'Seguís el sendero del río con el frasco de sal apretado en la mano y las palabras del sello repetidas en voz baja. El molino tiene luces detrás de las ventanas tapiadas y el canto ya empezó. Todavía hay tiempo.',
 NULL, 0, NULL),

('GUARDIA', 'El guardián de la puerta',
 'Un hombre corpulento con una túnica empapada te cierra el paso. Es Abner Hollis, el dueño del molino. Tiene un cuchillo de destripar pescado en la mano y los ojos vidriosos. «Esta noche no entra nadie que no haya visto el Ojo», dice.',
 NULL, 0, NULL),

('RITUAL', 'La rueda y el sello',
 'Dentro del molino, tres figuras encapuchadas cantan alrededor del foso de la rueda hidráulica. El profesor Ward está encadenado junto al borde, pálido pero vivo. En el agua negra del foso algo enorme se mueve y la superficie empieza a hincharse.',
 '1/1D6', 0, NULL),

('RITUAL_PREPARADO', 'La rueda y el sello',
 'Dentro del molino, tres figuras encapuchadas cantan alrededor del foso de la rueda. Ward está encadenado junto al borde. En el fondo, bajo el agua que se agita, alcanzás a ver la piedra del sello que describía el diario. Tenés la sal y las palabras.',
 '1/1D6', 0, NULL),

('RITUAL_CAOS', 'El Ojo se abre',
 'El canto se quiebra en gritos. El agua del foso se levanta como una ola y una masa gris, cubierta de ojos que parpadean a destiempo, asoma por encima de la rueda. Los sectarios caen de rodillas. Ward te grita que corras.',
 '1D3/1D10', 0, NULL),

('FINAL_VICTORIA', 'El sello se cierra',
 'La sal toca la piedra y las palabras invertidas salen de tu boca casi solas. El agua hierve, la masa gris se retuerce y se hunde con un sonido que no vas a olvidar nunca. Los sectarios huyen. Liberás a Ward, que llora en silencio. Al amanecer, el molino es solo un edificio viejo junto a un río tranquilo. Por ahora.',
 NULL, 1, 'victoria'),

('FINAL_RESCATE', 'Escape con Ward',
 'Arrastrás a Ward fuera del molino mientras la rueda se parte detrás de ustedes. Desde la orilla ven cómo el edificio se derrumba sobre el foso. Ward sobrevivió, pero ninguno de los dos sabe si lo que vive bajo el agua volvió a dormirse o solo está esperando.',
 NULL, 1, 'rescate'),

('FINAL_SOLO', 'Huida en la noche',
 'Corrés sin mirar atrás hasta que el canto queda lejos. Al amanecer volvés con la policía del condado: el molino está vacío y seco, sin rastros de Ward ni de los Hollis. Nadie te cree. Esa noche, y muchas otras, soñás con un ojo sin párpado.',
 NULL, 1, 'huida'),

('HUIDA_FINAL', 'Volver a casa',
 'Decidís que esto es demasiado. Tomás el último tren que sale de Haverford. Semanas después, un diario local informa que el profesor Elias Ward desapareció «durante una tormenta». Guardás su carta en un cajón y no volvés a abrirlo.',
 NULL, 1, 'huida'),

('FINAL_MUERTE', 'Fin del camino',
 'Tus heridas son demasiado graves. Lo último que oís es el agua que sube y el canto que sigue y sigue. Tu investigador ha muerto.',
 NULL, 1, 'muerte'),

('FINAL_LOCURA', 'La mente se quiebra',
 'Algo se rompe dentro de tu cabeza. Te encuentran días después caminando por la playa, repitiendo palabras en una lengua que nadie conoce. Tu investigador ha perdido la razón para siempre.',
 NULL, 1, 'locura');

-- ---------- Opciones ----------
INSERT INTO opciones (escena_codigo, orden, texto, habilidad, dificultad, escena_exito, escena_fallo, texto_exito, texto_fallo, dano_fallo, cordura_fallo) VALUES
('INICIO', 1, 'Golpear la puerta de la vecina y pedirle ayuda', 'Persuasión', 'Normal', 'VECINA', 'CASA_EXTERIOR',
 'La anciana duda, pero tu tono amable la convence.', 'La señora Pell te mira con desconfianza y cierra la cortina. Vas a tener que arreglártelas solo.', NULL, NULL),
('INICIO', 2, 'Rodear la casa y buscar otra entrada', NULL, 'Normal', 'CASA_EXTERIOR', NULL, NULL, NULL, NULL, NULL),

('VECINA', 1, 'Usar la llave y entrar a la casa de Ward', NULL, 'Normal', 'ESTUDIO', NULL, NULL, NULL, NULL, NULL),
('VECINA', 2, 'Ir directamente al molino de los Hollis', NULL, 'Normal', 'MOLINO_CAMINO', NULL, NULL, NULL, NULL, NULL),

('CASA_EXTERIOR', 1, 'Forzar la cerradura de la cocina', 'Cerrajería', 'Normal', 'ESTUDIO', 'ESTUDIO',
 'La cerradura cede con un clic.', 'La ganzúa se rompe. Terminás rompiendo el vidrio con el codo y te cortás.', '1D3', NULL),
('CASA_EXTERIOR', 2, 'Trepar al balcón del estudio', 'Trepar', 'Normal', 'ESTUDIO', 'ESTUDIO',
 'Subís por la enredadera y entrás por la ventana.', 'La madera podrida se quiebra y caés de espaldas. Dolorido, volvés a intentarlo y esta vez lo lográs.', '1D6', NULL),

('ESTUDIO', 1, 'Revisar los libros y papeles del escritorio', 'Buscar libros', 'Normal', 'DIARIO', 'ESTUDIO_TARDE',
 'Entre el desorden aparece un cuaderno de tapas de cuero.', 'Hay demasiados papeles y ninguno parece importante.', NULL, NULL),
('ESTUDIO', 2, 'Examinar la habitación en busca de algo fuera de lugar', 'Descubrir', 'Normal', 'TRAMPILLA', 'ESTUDIO_TARDE',
 'La alfombra corrida esconde algo.', 'No ves nada fuera de lo común.', NULL, NULL),

('ESTUDIO_TARDE', 1, 'Seguir el sonido con atención', 'Escuchar', 'Normal', 'TRAMPILLA', 'MOLINO_CAMINO',
 'El ruido viene de debajo de la alfombra.', 'El ruido se detiene. Recordás la mención del molino y salís a la lluvia.', NULL, NULL),
('ESTUDIO_TARDE', 2, 'Salir de la casa e ir al molino', NULL, 'Normal', 'MOLINO_CAMINO', NULL, NULL, NULL, NULL, NULL),

('DIARIO', 1, 'Traducir el párrafo en latín', 'Otras lenguas', 'Difícil', 'DIARIO_TRADUCIDO', 'MOLINO_CAMINO',
 'Las palabras empiezan a tener sentido.', 'El latín de Ward es demasiado oscuro para vos. Guardás el diario y salís hacia el molino.', NULL, NULL),
('DIARIO', 2, 'No perder tiempo e ir al molino', NULL, 'Normal', 'MOLINO_CAMINO', NULL, NULL, NULL, NULL, NULL),

('DIARIO_TRADUCIDO', 1, 'Ir al molino antes de la marea alta', NULL, 'Normal', 'MOLINO_PREPARADO', NULL, NULL, NULL, NULL, NULL),

('TRAMPILLA', 1, 'Bajar en silencio con la lámpara', 'Sigilo', 'Normal', 'TUNEL', 'TUNEL_CRIATURA',
 'Avanzás sin hacer ruido.', 'Tus pasos resuenan en el agua. Algo te escuchó.', NULL, NULL),
('TRAMPILLA', 2, 'Cerrar la trampilla y abandonar Haverford', NULL, 'Normal', 'HUIDA_FINAL', NULL, NULL, NULL, NULL, NULL),

('TUNEL', 1, 'Subir la escalera hacia la luz', NULL, 'Normal', 'RITUAL', NULL, NULL, NULL, NULL, NULL),

('TUNEL_CRIATURA', 1, 'Zafarte con un movimiento rápido', 'Esquivar', 'Normal', 'TUNEL', 'TUNEL',
 'Te soltás y retrocedés contra la pared.', 'La cosa te arrastra unos metros contra las piedras antes de soltarte.', '1D6', NULL),
('TUNEL_CRIATURA', 2, 'Golpearla con todas tus fuerzas', 'Combatir (Pelea)', 'Normal', 'TUNEL', 'TUNEL',
 'La golpeás y se hunde con un chillido.', 'Tus golpes no le hacen nada; te muerde antes de desaparecer.', '1D6+1', '1'),

('MOLINO_CAMINO', 1, 'Acercarte sigilosamente por la parte trasera', 'Sigilo', 'Normal', 'RITUAL', 'GUARDIA',
 'Te deslizás por una puerta lateral sin que nadie te vea.', 'Pisás una tabla suelta. Alguien viene.', NULL, NULL),
('MOLINO_CAMINO', 2, 'Entrar por la puerta principal', NULL, 'Normal', 'GUARDIA', NULL, NULL, NULL, NULL, NULL),

('MOLINO_PREPARADO', 1, 'Acercarte sigilosamente por la parte trasera', 'Sigilo', 'Normal', 'RITUAL_PREPARADO', 'GUARDIA',
 'Te deslizás por una puerta lateral sin que nadie te vea.', 'Pisás una tabla suelta. Alguien viene.', NULL, NULL),

('GUARDIA', 1, 'Convencerlo de que sos un iniciado', 'Charlatanería', 'Difícil', 'RITUAL', 'RITUAL',
 'Hollis baja el cuchillo y te deja pasar.', 'No te cree. Forcejean y te hace un corte antes de que lo empujes al agua.', '1D4', NULL),
('GUARDIA', 2, 'Atacarlo antes de que reaccione', 'Combatir (Pelea)', 'Normal', 'RITUAL', 'RITUAL',
 'Lo derribás de un golpe.', 'Te corta en el brazo, pero finalmente cae.', '1D4+2', NULL),
('GUARDIA', 3, 'Darte la vuelta y huir', NULL, 'Normal', 'HUIDA_FINAL', NULL, NULL, NULL, NULL, NULL),

('RITUAL', 1, 'Liberar a Ward mientras los sectarios cantan', 'Sigilo', 'Difícil', 'FINAL_RESCATE', 'RITUAL_CAOS',
 'Abrís las cadenas sin que nadie lo note.', 'Uno de los encapuchados te ve y grita.', NULL, NULL),
('RITUAL', 2, 'Interrumpir el canto a los gritos', 'Intimidar', 'Normal', 'RITUAL_CAOS', 'RITUAL_CAOS',
 'Los sectarios retroceden aterrados.', 'Se lanzan contra vos antes de que el caos estalle.', '1D6', NULL),
('RITUAL', 3, 'Retroceder y huir', NULL, 'Normal', 'FINAL_SOLO', NULL, NULL, NULL, NULL, NULL),

('RITUAL_PREPARADO', 1, 'Arrojar la sal sobre el sello y recitar las palabras', 'POD', 'Normal', 'FINAL_VICTORIA', 'RITUAL_CAOS',
 'Tu voluntad no flaquea.', 'Las palabras se te traban en la garganta. El Ojo te mira.', NULL, '1D4'),
('RITUAL_PREPARADO', 2, 'Liberar a Ward mientras los sectarios cantan', 'Sigilo', 'Difícil', 'FINAL_RESCATE', 'RITUAL_CAOS',
 'Abrís las cadenas sin que nadie lo note.', 'Uno de los encapuchados te ve y grita.', NULL, NULL),

('RITUAL_CAOS', 1, 'Arrastrar a Ward hacia la salida', 'FUE', 'Normal', 'FINAL_RESCATE', 'FINAL_SOLO',
 'Lo cargás sobre el hombro y salís.', 'No podés moverlo. Algo te golpea y te arroja hacia la puerta.', '1D6', NULL),
('RITUAL_CAOS', 2, 'Correr sin mirar atrás', 'SUERTE', 'Normal', 'FINAL_SOLO', 'FINAL_SOLO',
 'La suerte te acompaña: encontrás la salida.', 'Una viga cae sobre vos mientras huís.', '2D6', NULL);
