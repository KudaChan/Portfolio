import React from 'react'
import { FaLocationArrow } from "react-icons/fa6";
import { Spotlight } from './ui/Spotlight'
import { TextGenerateEffect } from './ui/TextGenerateEffect'
import { LayoutGrid } from './ui/LayoutGrid'
import { cards } from '@/data'
import MagicButton from './ui/MagicButton';

const Hero = () => {
    return (
        <div className="pb-20 sm:pt-36 md:pt-0" id="home">
            <Spotlight
                className="-top-40 -left-10 md:-left-32 md:-top-20 h-screen"
                fill="white"
            />
            <Spotlight
                className="h-[80vh] w-[50vw] top-10 left-full"
                fill="purple"
            />
            <Spotlight className="left-80 top-28 h-[80vh] w-[50vw]" fill="blue" />

            <div className="h-screen w-full dark:bg-black-100 bg-white  dark:bg-dot-white/[0.15] bg-dot-black/[0.2] absolute flex items-center justify-center">
                <div className="absolute pointer-events-none inset-0 flex items-center justify-center dark:bg-black-100 bg-white [mask-image:radial-gradient(ellipse_at_center,transparent_10%,black)]" />
            </div>
            <div className="flex justify-center relative my-20 z-10">
                <div className="max-w-[89vw] md:max-w-2xl lg:max-w-[60vw] flex flex-col items-center justify-center">
                    <h2 className="uppercase tracking-widest text-xs text-center text-blue-100 max-w-80">
                        Building Scalable Dynamic Applications With modern Tools.
                    </h2>
                    <TextGenerateEffect
                        words="Transforming Concepts into Seamless User Experiences"
                        className="text-center text-[40px] md:text-5xl lg:text-6xl"
                    />
                    <p className="text-center md:tracking-wider mb-4 text-sm md:text-lg lg:text-2xl">
                        Hi! I&apos;m Chandan, a Full-Stack Developer based in Delhi (INDIA).
                    </p>
                    <div>
                        <a href="#about" className='mr-10 mt-5'>
                            <MagicButton
                                title="Show my work"
                                icon={<FaLocationArrow />}
                                position="right"
                                otherClasses='hover:-translate-y-1 transition duration-400'
                            />
                        </a>
                        <a href="#contact ">
                            <MagicButton
                                title="Contact me"
                                icon={<FaLocationArrow />}
                                position="right"
                                otherClasses='bg-slate-950'
                            />
                        </a>
                    </div>

                </div>
                <div className="h-[80vh] py-20 w-full ml-10 hidden lg:block">
                    <LayoutGrid cards={cards} />
                </div>
            </div>
        </div>
    )
}

export default Hero