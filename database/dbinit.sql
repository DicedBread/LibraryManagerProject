--
-- PostgreSQL database dump
--

-- Dumped from database version 16.2 (Debian 16.2-1.pgdg120+2)
-- Dumped by pg_dump version 16.2

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
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Name: authours; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.authours (
    authour_id bigint NOT NULL,
    authour character varying(500)
);


ALTER TABLE public.authours OWNER TO postgres;

--
-- Name: authours_authour_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.authours_authour_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.authours_authour_id_seq OWNER TO postgres;

--
-- Name: authours_authour_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.authours_authour_id_seq OWNED BY public.authours.authour_id;


--
-- Name: books; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.books (
    isbn character varying(50) NOT NULL,
    title character varying(500),
    img_url character varying(200),
    authour_id bigint DEFAULT 1 NOT NULL,
    publisher_id bigint DEFAULT 1 NOT NULL,
    text_search tsvector GENERATED ALWAYS AS (to_tsvector('english'::regconfig, (title)::text)) STORED
);


ALTER TABLE public.books OWNER TO postgres;

--
-- Name: loans; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.loans (
    loan_id bigint NOT NULL,
    user_id bigint NOT NULL,
    isbn character varying(50) NOT NULL,
    date date NOT NULL
);


ALTER TABLE public.loans OWNER TO postgres;

--
-- Name: loans_loan_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.loans_loan_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.loans_loan_id_seq OWNER TO postgres;

--
-- Name: loans_loan_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.loans_loan_id_seq OWNED BY public.loans.loan_id;


--
-- Name: publishers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.publishers (
    publisher_id bigint NOT NULL,
    publisher character varying(200)
);


ALTER TABLE public.publishers OWNER TO postgres;

--
-- Name: publishers_publisher_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.publishers_publisher_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.publishers_publisher_id_seq OWNER TO postgres;

--
-- Name: publishers_publisher_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.publishers_publisher_id_seq OWNED BY public.publishers.publisher_id;


--
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    user_id bigint NOT NULL,
    email character varying(100) NOT NULL,
    username character varying(100) NOT NULL,
    password character varying(200) NOT NULL
);


ALTER TABLE public.users OWNER TO postgres;

--
-- Name: users_user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.users_user_id_seq OWNER TO postgres;

--
-- Name: users_user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_user_id_seq OWNED BY public.users.user_id;


--
-- Name: authours authour_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.authours ALTER COLUMN authour_id SET DEFAULT nextval('public.authours_authour_id_seq'::regclass);


--
-- Name: loans loan_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.loans ALTER COLUMN loan_id SET DEFAULT nextval('public.loans_loan_id_seq'::regclass);


--
-- Name: publishers publisher_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.publishers ALTER COLUMN publisher_id SET DEFAULT nextval('public.publishers_publisher_id_seq'::regclass);


--
-- Name: users user_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN user_id SET DEFAULT nextval('public.users_user_id_seq'::regclass);


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20250213012721_InitCreate	9.0.2
\.


--
-- Data for Name: authours; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.authours (authour_id, authour) FROM stdin;
1	Mark P. O. Morford
2	Richard Bruce Wright
3	Carlo D'Este
4	Gina Bari Kolata
5	E. J. W. Barber
6	Amy Tan
7	Robert Cowley
8	Scott Turow
9	David Cordingly
10	Ann Beattie
11	David Adams Richards
12	Adam Lebor
13	Sheila Heti
14	R. J. Kaiser
15	Jack Canfield
16	Loren D. Estleman
17	Robert Hendrickson
18	Julia Oliver
19	John Grisham
20	Toni Morrison
\.


--
-- Data for Name: books; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.books (isbn, title, img_url, authour_id, publisher_id) FROM stdin;
0195153448	Classical Mythology	http://images.amazon.com/images/P/0195153448.01.MZZZZZZZ.jpg	1	1
0002005018	Clara Callan	http://images.amazon.com/images/P/0002005018.01.MZZZZZZZ.jpg	2	2
0060973129	Decision in Normandy	http://images.amazon.com/images/P/0060973129.01.MZZZZZZZ.jpg	3	3
0374157065	Flu: The Story of the Great Influenza Pandemic of 1918 and the Search for the Virus That Caused It	http://images.amazon.com/images/P/0374157065.01.MZZZZZZZ.jpg	4	4
0393045218	The Mummies of Urumchi	http://images.amazon.com/images/P/0393045218.01.MZZZZZZZ.jpg	5	5
0399135782	The Kitchen God's Wife	http://images.amazon.com/images/P/0399135782.01.MZZZZZZZ.jpg	6	6
0425176428	What If?: The World's Foremost Military Historians Imagine What Might Have Been	http://images.amazon.com/images/P/0425176428.01.MZZZZZZZ.jpg	7	7
0671870432	PLEADING GUILTY	http://images.amazon.com/images/P/0671870432.01.MZZZZZZZ.jpg	8	8
0679425608	Under the Black Flag: The Romance and the Reality of Life Among the Pirates	http://images.amazon.com/images/P/0679425608.01.MZZZZZZZ.jpg	9	9
074322678X	Where You'll Find Me: And Other Stories	http://images.amazon.com/images/P/074322678X.01.MZZZZZZZ.jpg	10	10
0771074670	Nights Below Station Street	http://images.amazon.com/images/P/0771074670.01.MZZZZZZZ.jpg	11	11
080652121X	Hitler's Secret Bankers: The Myth of Swiss Neutrality During the Holocaust	http://images.amazon.com/images/P/080652121X.01.MZZZZZZZ.jpg	12	12
0887841740	The Middle Stories	http://images.amazon.com/images/P/0887841740.01.MZZZZZZZ.jpg	13	13
1552041778	Jane Doe	http://images.amazon.com/images/P/1552041778.01.MZZZZZZZ.jpg	14	14
1558746218	A Second Chicken Soup for the Woman's Soul (Chicken Soup for the Soul Series)	http://images.amazon.com/images/P/1558746218.01.MZZZZZZZ.jpg	15	15
1567407781	The Witchfinder (Amos Walker Mystery Series)	http://images.amazon.com/images/P/1567407781.01.MZZZZZZZ.jpg	16	16
1575663937	More Cunning Than Man: A Social History of Rats and Man	http://images.amazon.com/images/P/1575663937.01.MZZZZZZZ.jpg	17	17
1881320189	Goodbye to the Buttermilk Sky	http://images.amazon.com/images/P/1881320189.01.MZZZZZZZ.jpg	18	18
0440234743	The Testament	http://images.amazon.com/images/P/0440234743.01.MZZZZZZZ.jpg	19	19
0452264464	Beloved (Plume Contemporary Fiction)	http://images.amazon.com/images/P/0452264464.01.MZZZZZZZ.jpg	20	20
\.


--
-- Data for Name: loans; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.loans (loan_id, user_id, isbn, date) FROM stdin;
1	1	0060973129	2024-12-23
2	1	0393045218	2024-12-23
\.


--
-- Data for Name: publishers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.publishers (publisher_id, publisher) FROM stdin;
1	Oxford University Press
2	HarperFlamingo Canada
3	HarperPerennial
4	Farrar Straus Giroux
5	W. W. Norton &amp; Company
6	Putnam Pub Group
7	Berkley Publishing Group
8	Audioworks
9	Random House
10	Scribner
11	Emblem Editions
12	Citadel Press
13	House of Anansi Press
14	Mira Books
15	Health Communications
16	Brilliance Audio - Trade
17	Kensington Publishing Corp.
18	River City Pub
19	Dell
20	Plume
\.


--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (user_id, email, username, password) FROM stdin;
1	test@test	test	AQAAAAIAAYagAAAAEIgJya5EiBidQjAtn07PZk8mJOUa6RN32aCyFkJQdjusuqjQs4hzFAKtDtYJZzCKOw==
2	test2@test	test2	AQAAAAIAAYagAAAAEEm2doi8/1UlMGigET0GpoyrDqJt2AfD2DJbf7Jsjrfi0JpomGXndUqnZcNDp+Rb+g==
\.


--
-- Name: authours_authour_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.authours_authour_id_seq', 20, true);


--
-- Name: loans_loan_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.loans_loan_id_seq', 2, true);


--
-- Name: publishers_publisher_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.publishers_publisher_id_seq', 20, true);


--
-- Name: users_user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_user_id_seq', 2, true);


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: authours authours_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.authours
    ADD CONSTRAINT authours_pkey PRIMARY KEY (authour_id);


--
-- Name: books books_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.books
    ADD CONSTRAINT books_pkey PRIMARY KEY (isbn);


--
-- Name: loans loans_pkey1; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.loans
    ADD CONSTRAINT loans_pkey1 PRIMARY KEY (loan_id);


--
-- Name: loans loans_user_id_isbn_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.loans
    ADD CONSTRAINT loans_user_id_isbn_key UNIQUE (user_id, isbn);


--
-- Name: publishers publishers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.publishers
    ADD CONSTRAINT publishers_pkey PRIMARY KEY (publisher_id);


--
-- Name: users users_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_email_key UNIQUE (email);


--
-- Name: users users_pkey1; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey1 PRIMARY KEY (user_id);


--
-- Name: text_search_idx; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX text_search_idx ON public.books USING gin (text_search);


--
-- Name: books fk_authours; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.books
    ADD CONSTRAINT fk_authours FOREIGN KEY (authour_id) REFERENCES public.authours(authour_id) ON UPDATE CASCADE;


--
-- Name: books fk_publisher; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.books
    ADD CONSTRAINT fk_publisher FOREIGN KEY (publisher_id) REFERENCES public.publishers(publisher_id) ON UPDATE CASCADE;


--
-- Name: loans loans_isbn_fkey1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.loans
    ADD CONSTRAINT loans_isbn_fkey1 FOREIGN KEY (isbn) REFERENCES public.books(isbn);


--
-- Name: loans loans_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.loans
    ADD CONSTRAINT loans_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(user_id);


--
-- PostgreSQL database dump complete
--

