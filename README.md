# AnatoVerse

Inspiration
Understanding complex 3D anatomical structures using traditional 2D scans presents significant challenges in medical education and diagnostics. We envisioned AnatoVerse to bridge this gap by transforming medical imaging into an interactive VR experience—bringing precision, intuition, and immersion to anatomical visualization.

What It Does
AnatoVerse is a VR-integrated platform that converts brain MRI data into interactive 3D models, allowing real-time exploration and segmentation:
- Brain Tumor Segmentation – Developed an optimized 3D UNet in PyTorch for MRI-based tumor segmentation in the brain.
- 3D Mesh Generation – Applied the marching cubes algorithm to reconstruct detailed 3D surface meshes of segmented tumor regions and brain structures for enhanced visualization.
- VR Interaction – Imported models into Unity, enabling hand-tracking-based manipulation (pinch-to-drag, zoom, rotate) for intuitive exploration of the brain and tumor.
- Multi-Organ Expansion – Extended interactive capabilities to heart, eye, lung, liver, and stomach models, enhancing the platform's medical training potential.
