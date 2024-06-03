--
-- PostgreSQL database dump
--

-- Dumped from database version 12.17 (Ubuntu 12.17-1.pgdg22.04+1)
-- Dumped by pg_dump version 12.17 (Ubuntu 12.17-1.pgdg22.04+1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

DROP DATABASE number_guess;
--
-- Name: number_guess; Type: DATABASE; Schema: -; Owner: freecodecamp
--

CREATE DATABASE number_guess WITH TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'C.UTF-8' LC_CTYPE = 'C.UTF-8';


ALTER DATABASE number_guess OWNER TO freecodecamp;

\connect number_guess

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: games; Type: TABLE; Schema: public; Owner: freecodecamp
--

CREATE TABLE public.games (
    game_id integer NOT NULL,
    user_id integer NOT NULL,
    score integer DEFAULT 0 NOT NULL
);


ALTER TABLE public.games OWNER TO freecodecamp;

--
-- Name: games_game_id_seq; Type: SEQUENCE; Schema: public; Owner: freecodecamp
--

CREATE SEQUENCE public.games_game_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.games_game_id_seq OWNER TO freecodecamp;

--
-- Name: games_game_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: freecodecamp
--

ALTER SEQUENCE public.games_game_id_seq OWNED BY public.games.game_id;


--
-- Name: users; Type: TABLE; Schema: public; Owner: freecodecamp
--

CREATE TABLE public.users (
    user_id integer NOT NULL,
    name character varying(22) NOT NULL,
    game_played integer DEFAULT 0 NOT NULL
);


ALTER TABLE public.users OWNER TO freecodecamp;

--
-- Name: users_user_id_seq; Type: SEQUENCE; Schema: public; Owner: freecodecamp
--

CREATE SEQUENCE public.users_user_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.users_user_id_seq OWNER TO freecodecamp;

--
-- Name: users_user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: freecodecamp
--

ALTER SEQUENCE public.users_user_id_seq OWNED BY public.users.user_id;


--
-- Name: games game_id; Type: DEFAULT; Schema: public; Owner: freecodecamp
--

ALTER TABLE ONLY public.games ALTER COLUMN game_id SET DEFAULT nextval('public.games_game_id_seq'::regclass);


--
-- Name: users user_id; Type: DEFAULT; Schema: public; Owner: freecodecamp
--

ALTER TABLE ONLY public.users ALTER COLUMN user_id SET DEFAULT nextval('public.users_user_id_seq'::regclass);


--
-- Data for Name: games; Type: TABLE DATA; Schema: public; Owner: freecodecamp
--

INSERT INTO public.games VALUES (1, 1, 10);
INSERT INTO public.games VALUES (2, 1, 10);
INSERT INTO public.games VALUES (3, 2, 119);
INSERT INTO public.games VALUES (4, 2, 967);
INSERT INTO public.games VALUES (5, 3, 271);
INSERT INTO public.games VALUES (6, 3, 825);
INSERT INTO public.games VALUES (7, 2, 4);
INSERT INTO public.games VALUES (8, 2, 156);
INSERT INTO public.games VALUES (9, 2, 632);
INSERT INTO public.games VALUES (10, 4, 984);
INSERT INTO public.games VALUES (11, 4, 122);
INSERT INTO public.games VALUES (12, 5, 622);
INSERT INTO public.games VALUES (13, 5, 540);
INSERT INTO public.games VALUES (14, 4, 527);
INSERT INTO public.games VALUES (15, 4, 4);
INSERT INTO public.games VALUES (16, 4, 751);
INSERT INTO public.games VALUES (17, 6, 966);
INSERT INTO public.games VALUES (18, 6, 703);
INSERT INTO public.games VALUES (19, 7, 248);
INSERT INTO public.games VALUES (20, 7, 506);
INSERT INTO public.games VALUES (21, 6, 976);
INSERT INTO public.games VALUES (22, 6, 649);
INSERT INTO public.games VALUES (23, 6, 98);
INSERT INTO public.games VALUES (24, 8, 893);
INSERT INTO public.games VALUES (25, 8, 37);
INSERT INTO public.games VALUES (26, 9, 705);
INSERT INTO public.games VALUES (27, 9, 597);
INSERT INTO public.games VALUES (28, 8, 542);
INSERT INTO public.games VALUES (29, 8, 979);
INSERT INTO public.games VALUES (30, 8, 145);
INSERT INTO public.games VALUES (31, 10, 149);
INSERT INTO public.games VALUES (32, 10, 620);
INSERT INTO public.games VALUES (33, 11, 222);
INSERT INTO public.games VALUES (34, 11, 306);
INSERT INTO public.games VALUES (35, 10, 744);
INSERT INTO public.games VALUES (36, 10, 989);
INSERT INTO public.games VALUES (37, 10, 271);
INSERT INTO public.games VALUES (38, 12, 200);
INSERT INTO public.games VALUES (39, 12, 722);
INSERT INTO public.games VALUES (40, 13, 564);
INSERT INTO public.games VALUES (41, 13, 759);
INSERT INTO public.games VALUES (42, 12, 10);
INSERT INTO public.games VALUES (43, 12, 746);
INSERT INTO public.games VALUES (44, 12, 790);
INSERT INTO public.games VALUES (45, 14, 368);
INSERT INTO public.games VALUES (46, 14, 665);
INSERT INTO public.games VALUES (47, 15, 940);
INSERT INTO public.games VALUES (48, 15, 331);
INSERT INTO public.games VALUES (49, 14, 508);
INSERT INTO public.games VALUES (50, 14, 140);
INSERT INTO public.games VALUES (51, 14, 33);
INSERT INTO public.games VALUES (52, 16, 939);
INSERT INTO public.games VALUES (53, 16, 586);
INSERT INTO public.games VALUES (54, 17, 829);
INSERT INTO public.games VALUES (55, 17, 898);
INSERT INTO public.games VALUES (56, 16, 378);
INSERT INTO public.games VALUES (57, 16, 825);
INSERT INTO public.games VALUES (58, 16, 860);
INSERT INTO public.games VALUES (59, 18, 940);
INSERT INTO public.games VALUES (60, 18, 812);
INSERT INTO public.games VALUES (61, 19, 600);
INSERT INTO public.games VALUES (62, 19, 715);
INSERT INTO public.games VALUES (63, 18, 538);
INSERT INTO public.games VALUES (64, 18, 655);
INSERT INTO public.games VALUES (65, 18, 818);
INSERT INTO public.games VALUES (66, 20, 551);
INSERT INTO public.games VALUES (67, 20, 12);
INSERT INTO public.games VALUES (68, 21, 231);
INSERT INTO public.games VALUES (69, 21, 332);
INSERT INTO public.games VALUES (70, 20, 46);
INSERT INTO public.games VALUES (71, 20, 72);
INSERT INTO public.games VALUES (72, 20, 629);
INSERT INTO public.games VALUES (73, 22, 739);
INSERT INTO public.games VALUES (74, 22, 361);
INSERT INTO public.games VALUES (75, 23, 1000);
INSERT INTO public.games VALUES (76, 23, 561);
INSERT INTO public.games VALUES (77, 22, 78);
INSERT INTO public.games VALUES (78, 22, 37);
INSERT INTO public.games VALUES (79, 22, 773);
INSERT INTO public.games VALUES (80, 24, 201);
INSERT INTO public.games VALUES (81, 24, 306);
INSERT INTO public.games VALUES (82, 25, 327);
INSERT INTO public.games VALUES (83, 25, 554);
INSERT INTO public.games VALUES (84, 24, 633);
INSERT INTO public.games VALUES (85, 24, 427);
INSERT INTO public.games VALUES (86, 24, 128);
INSERT INTO public.games VALUES (87, 1, 2);
INSERT INTO public.games VALUES (88, 26, 176);
INSERT INTO public.games VALUES (89, 26, 942);
INSERT INTO public.games VALUES (90, 27, 175);
INSERT INTO public.games VALUES (91, 27, 136);
INSERT INTO public.games VALUES (92, 26, 968);
INSERT INTO public.games VALUES (93, 26, 522);
INSERT INTO public.games VALUES (94, 26, 805);
INSERT INTO public.games VALUES (95, 1, 1);
INSERT INTO public.games VALUES (96, 28, 411);
INSERT INTO public.games VALUES (97, 28, 605);
INSERT INTO public.games VALUES (98, 29, 297);
INSERT INTO public.games VALUES (99, 29, 64);
INSERT INTO public.games VALUES (100, 28, 232);
INSERT INTO public.games VALUES (101, 28, 391);
INSERT INTO public.games VALUES (102, 28, 164);
INSERT INTO public.games VALUES (103, 30, 95);
INSERT INTO public.games VALUES (104, 30, 656);
INSERT INTO public.games VALUES (105, 31, 944);
INSERT INTO public.games VALUES (106, 31, 428);
INSERT INTO public.games VALUES (107, 30, 363);
INSERT INTO public.games VALUES (108, 30, 952);
INSERT INTO public.games VALUES (109, 30, 115);
INSERT INTO public.games VALUES (110, 32, 199);
INSERT INTO public.games VALUES (111, 32, 840);
INSERT INTO public.games VALUES (112, 33, 194);
INSERT INTO public.games VALUES (113, 33, 150);
INSERT INTO public.games VALUES (114, 32, 177);
INSERT INTO public.games VALUES (115, 32, 480);
INSERT INTO public.games VALUES (116, 32, 883);


--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: freecodecamp
--

INSERT INTO public.users VALUES (3, 'user_1717368893555', 2);
INSERT INTO public.users VALUES (2, 'user_1717368893556', 5);
INSERT INTO public.users VALUES (25, 'user_1717371198183', 2);
INSERT INTO public.users VALUES (5, 'user_1717368944060', 2);
INSERT INTO public.users VALUES (4, 'user_1717368944061', 5);
INSERT INTO public.users VALUES (24, 'user_1717371198184', 5);
INSERT INTO public.users VALUES (7, 'user_1717368988428', 2);
INSERT INTO public.users VALUES (6, 'user_1717368988429', 5);
INSERT INTO public.users VALUES (9, 'user_1717369085559', 2);
INSERT INTO public.users VALUES (27, 'user_1717371625086', 2);
INSERT INTO public.users VALUES (8, 'user_1717369085560', 5);
INSERT INTO public.users VALUES (11, 'user_1717369099594', 2);
INSERT INTO public.users VALUES (26, 'user_1717371625087', 5);
INSERT INTO public.users VALUES (10, 'user_1717369099595', 5);
INSERT INTO public.users VALUES (1, 'chandan', 4);
INSERT INTO public.users VALUES (13, 'user_1717369156278', 2);
INSERT INTO public.users VALUES (12, 'user_1717369156279', 5);
INSERT INTO public.users VALUES (15, 'user_1717369185306', 2);
INSERT INTO public.users VALUES (29, 'user_1717371799563', 2);
INSERT INTO public.users VALUES (14, 'user_1717369185307', 5);
INSERT INTO public.users VALUES (17, 'user_1717369203286', 2);
INSERT INTO public.users VALUES (28, 'user_1717371799564', 5);
INSERT INTO public.users VALUES (16, 'user_1717369203287', 5);
INSERT INTO public.users VALUES (19, 'user_1717369326749', 2);
INSERT INTO public.users VALUES (31, 'user_1717371842876', 2);
INSERT INTO public.users VALUES (18, 'user_1717369326750', 5);
INSERT INTO public.users VALUES (21, 'user_1717369784817', 2);
INSERT INTO public.users VALUES (30, 'user_1717371842877', 5);
INSERT INTO public.users VALUES (20, 'user_1717369784818', 5);
INSERT INTO public.users VALUES (23, 'user_1717371066075', 2);
INSERT INTO public.users VALUES (22, 'user_1717371066076', 5);
INSERT INTO public.users VALUES (33, 'user_1717371899981', 2);
INSERT INTO public.users VALUES (32, 'user_1717371899982', 5);


--
-- Name: games_game_id_seq; Type: SEQUENCE SET; Schema: public; Owner: freecodecamp
--

SELECT pg_catalog.setval('public.games_game_id_seq', 116, true);


--
-- Name: users_user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: freecodecamp
--

SELECT pg_catalog.setval('public.users_user_id_seq', 33, true);


--
-- Name: games games_pkey; Type: CONSTRAINT; Schema: public; Owner: freecodecamp
--

ALTER TABLE ONLY public.games
    ADD CONSTRAINT games_pkey PRIMARY KEY (game_id);


--
-- Name: users users_name_key; Type: CONSTRAINT; Schema: public; Owner: freecodecamp
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_name_key UNIQUE (name);


--
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: freecodecamp
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (user_id);


--
-- Name: games games_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: freecodecamp
--

ALTER TABLE ONLY public.games
    ADD CONSTRAINT games_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(user_id);


--
-- PostgreSQL database dump complete
--

