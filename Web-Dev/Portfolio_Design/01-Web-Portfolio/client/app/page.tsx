"use client";

import Grid from "@/components/Grid";
import Hero from "@/components/Hero";
import RecentProject from "@/components/RecentProject";
import { FloatingNav } from "@/components/ui/FloatingNavbar";
import { navItems } from "@/data";

const Home = () => {
  return (
    <main className="relative bg-black-100 flex justify-center items-center flex-col overflow-hidden mx-auto sm:px-10 px-5">
      <div className="max-w-[85vw] w-full">
        <FloatingNav navItems={navItems} />
        <Hero />
        <Grid />
        <RecentProject />
      </div>
    </main>
  );
}

export default Home;